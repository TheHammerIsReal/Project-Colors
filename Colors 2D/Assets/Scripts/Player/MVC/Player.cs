using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour , IDamageable
{
    PlayerModel _model;
    PlayerController _controller;
    PlayerView _view;
    Player_Collisions _collisions;

    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float SecondJumpForce { get; private set; }
    [field: SerializeField] public float DashForce { get; private set; }
    [field: SerializeField] public float WallSlidingSpeed { get; private set; }
    [field: SerializeField] public float DashCooldown { get; private set; }
    [field: SerializeField] public float DashForceY { get; private set; }
    [field: SerializeField] public Vector3 WallJumpForce { get; private set;}
    [field: SerializeField] public float WallJumpDuration { get; private set; }
    [field: SerializeField] public float CoyoteTimer { get; private set; }
    [field: SerializeField] public int GroundDmg { get; private set; }
    [field: SerializeField] public float JumpCooldown { get; private set; }
    [field: SerializeField] public int GravityScaleGround { get; private set; }
    [field: SerializeField] public float SmoothWall { get; private set; }




    public event Action<ColorTone> OnColorChange = delegate { };
    public event Action<Vector3> OnMovement = delegate { };
    public ColorTone color;
    Rigidbody2D _rb;
    public List<ColorTin> inventory = new List<ColorTin>();
    public Color orange;
    public LayerMask wallsLayer = 1 << 7;

    [Header("UI")]
    public List<Image> nullImages = new List<Image>(), fillImages = new List<Image>();
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] GameObject _target;
    [SerializeField] float _radiusTarget;

    public Transform groundAttackBox;
    public LayerMask railLayer = 1<<8;
    public LayerMask groundLayer = 1 << 6;
    public Transform spawnPointShoot;
    Vector3 _initialPos;
    [SerializeField] Transform _testTransform;
    [HideInInspector] public bool smartBulletThrowed;
    Vector3 _mousePos;
    public Vector3 dir;
    bool _aim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _model = new PlayerModel(this, _rb);
        _view = new PlayerView(this, _renderer);
        _collisions = new Player_Collisions(_model, this, _rb);
        _controller = new PlayerController(_model , this, _collisions);

        _model.Awake();
        _view.BlindColors();

        #region Color Change
        OnColorChange += _view.ChangeSkin;
        OnColorChange += ChangeColor;
        OnColorChange += _model.ChangeAbility;
        #endregion

        _collisions.OnTinSaved += _model.SaveTin;
        _collisions.OnTinSaved += _view.ChangeUI;

        _model.OnJump += _collisions.CollisionJump;

        _collisions.OnLanded += _view.LandAnimation;
        OnMovement += _model.Move;
    }

    private void Start()
    {
        _initialPos = transform.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        _controller.InputListener(_model);
        _model.Update();


        if (_aim)
        {
            // transform.rotation = _controller.MouseController(transform, _mousePos);

            if(Gamepad.current!=null && Gamepad.current.rightStick.IsActuated())
            {
                _mousePos = _controller.MousePosition();
                Vector3 targetPos = transform.position + _mousePos.normalized * _radiusTarget;
                _view.TargetUpdate(_target, transform, targetPos, _radiusTarget, spawnPointShoot);
            }

            else
            {
                _mousePos = Camera.main.ScreenToWorldPoint(_controller.MousePosition());
                _view.TargetUpdate(_target, transform, _mousePos, _radiusTarget, spawnPointShoot);
            }


            dir = _target.transform.position - transform.position;
        }

        


        if (Input.GetKeyDown(KeyCode.T)) transform.position = _testTransform.position;

    }

    private void FixedUpdate()
    {
        OnMovement(_controller.dir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisions.OnCollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _collisions.OnCollisionExit(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collisions.OnTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collisions.OnTriggerExit(collision);
    }

    public int InventoryIndex()
    {
        if (inventory[0] == null || inventory[0] == default)
        {
            return 0;
        }

        if (inventory[1] == null || inventory[1] == default)
        {
            return 1;
        }

        return 2;
    }

    void ChangeColor(ColorTone newColor)
    {
        color = newColor;

        if (newColor == ColorTone.Red || newColor == ColorTone.Violet)
        {
            _aim = true;
            _view.TargetSwitch(_target, true);
        }

        else
        {
            _aim = false;
            _view.TargetSwitch(_target, false);
        }
    }

    public void ColorChange(InputAction.CallbackContext obj)
    {
        if (inventory[0] != null)
        {
            if (color == ColorTone.White)
            {
                OnColorChange(inventory[0].color);
                _collisions.RailCheck();
                _collisions.WallCheck();
                return;
            }

            if (inventory[1] != null)
            {
                if (color == inventory[0].color)
                {
                    OnColorChange(inventory[1].color);
                    _collisions.RailCheck();
                    _collisions.WallCheck();
                    var temp = inventory[0];
                    inventory[0] = inventory[1];
                    inventory[1] = temp;
                    _view.ExchangeImages(inventory);
                }
                else
                {
                    OnColorChange(inventory[0].color);
                    _collisions.RailCheck();
                    _collisions.WallCheck();
                    var temp = inventory[1];
                    inventory[1] = inventory[0];
                    inventory[0] = temp;
                    _view.ExchangeImages(inventory);
                }
            }

        }
    }

    public void ColorWaste()
    {
        if (inventory[1] != default)
        {
            inventory[0] = inventory[1];

            inventory[1] = default;

            _view.ChangeUI(inventory[0], 0);
            _view.DeleteTin(1);
        }

        else
        {
            inventory[0] = default;
            _view.DeleteTin(0);
        }


        if (inventory[0] != default) OnColorChange(inventory[0].color);

        else OnColorChange(ColorTone.White);

        _collisions.RailCheck();
        _collisions.WallCheck();
    }

    public void ColorMergeEvent(InputAction.CallbackContext obj)
    {
        if(InventoryIndex()==2) ColorMerge(inventory[0], inventory[1]);

    }


    void ColorMerge(ColorTin color1, ColorTin color2)
    {
        if (color1.colorType == ColorType.Primary && color2.colorType == ColorType.Primary)
        {
            ColorTin newTin = ColorManager.instance.Merge(color1.color, color2.color);
            if (newTin != default)
            {
                inventory[0] = newTin;
                if (inventory[1] != default)
                    inventory[1] = default;
                if (color != ColorTone.White)
                    OnColorChange(inventory[0].color);
                _view.ChangeUI(newTin, 0);
                _view.DeleteTin(1);
            }
        }
    }

    public void TakeDmg(int dmg)
    {
        transform.position = _testTransform.position;
    }
}
