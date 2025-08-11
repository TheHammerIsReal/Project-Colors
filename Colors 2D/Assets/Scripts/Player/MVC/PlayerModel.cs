using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerModel
{
    public Action<Player> currentAbility;
    float _speed;
    float _jumpForce;
    float _wallSlidingSpeed;
    float _wallJumpDuration;
    float _smoothWall;
    public float dashForce, dashCooldown, dashForceY;
    public event Action<ColorTone> OnJump = delegate { };
    public event Action<Vector3, Vector3> OnSmartShoot = delegate { };
    Dictionary<ColorTone, IAbility> _colorAbilities = new Dictionary<ColorTone, IAbility>();
    IAbility _dash, _diagonalDash, _secondJump, _shoot , _ground , _smartShoot;
    Player _player;
    Rigidbody2D _rb;
    public bool jump = true;
    public bool isDashing = false;
    public bool wallJump = false;
    public bool groundAttack = false;
    public bool wallSliding = false;
    public bool doubleJump = false;
    public bool dashHit = false;
    bool _floorRaycast;
    float _dirX;
    Vector3 _wallJumpForce;
    LayerMask _groundLayer;
    float _airTime;
    float _coyoteTimer;
    public float jumpTimer;
    float _jumpCooldown;
    float _velocityYRef;
    bool _wallJumpAgain = true;

    public PlayerModel(Player player , Rigidbody2D rb)
    {
        _player = player;
        _rb = rb;
        _speed = player.Speed;
        _jumpForce = player.JumpForce;
        _wallSlidingSpeed = player.WallSlidingSpeed;
        dashForce = player.DashForce;
        dashCooldown = player.DashCooldown;
        dashForceY = player.DashForceY;
        _wallJumpForce = player.WallJumpForce;
        _wallJumpDuration = player.WallJumpDuration;
        _dash = new HorizontalDash(rb, this);
        _diagonalDash = new DiagonalDash(this, rb);
        _secondJump = new SecondJump(rb, this);
        _shoot = new Shoot();
        _ground = new GroundAttack(rb, this);
        _smartShoot = new SmartShoot();
        _groundLayer = player.groundLayer;
        _coyoteTimer = player.CoyoteTimer;
        _jumpCooldown = player.JumpCooldown;
        _smoothWall = player.SmoothWall;
    }

    public void Awake()
    {
        #region Abilities
        _colorAbilities.Add(ColorTone.Blue, _dash);
        _colorAbilities.Add(ColorTone.Green, _diagonalDash);
        _colorAbilities.Add(ColorTone.White, default);
        _colorAbilities.Add(ColorTone.Yellow, _secondJump);
        _colorAbilities.Add(ColorTone.Red, _shoot);
        _colorAbilities.Add(ColorTone.Violet, _smartShoot);
        _colorAbilities.Add(ColorTone.Orange, _ground);
        #endregion
    }

    public void ChangeAbility(ColorTone color)
    {
        if (color != ColorTone.White)
            currentAbility = _colorAbilities[color].Ability;

        else currentAbility = null;
    }

    public void Update()
    {
        _floorRaycast = CoyoteTime(_player.transform, 0.5f, _groundLayer);

        if (!_floorRaycast)
        {
            _airTime += Time.deltaTime;

            if (_airTime >= _coyoteTimer) jump = false;
        }

        else
        {
            _airTime = 0;
            jump = true;
            jumpTimer = 0;
        }
    }

    public void Move(Vector3 dir)
    {
        if (!isDashing && !wallJump && !groundAttack && !wallSliding)
        {
            _rb.velocity = new Vector2(dir.x * _speed, _rb.velocity.y);
            if (dir != Vector3.zero)
            {
                _dirX = dir.x;
                _player.transform.right = dir;
            }
        }

        else if (wallJump && !jump && !groundAttack && wallSliding)
        {
            
            float newY = Mathf.SmoothDamp(_rb.velocity.y, -_wallSlidingSpeed, ref _velocityYRef, _smoothWall);
            _rb.velocity = new Vector2(_rb.velocity.x, newY);
        }

    }

    public void Ability(InputAction.CallbackContext obj)
    {
        if(currentAbility!=null)
        currentAbility(_player);
    }

    public void Jump()
    {
        if (jump && !wallJump && !groundAttack)
        {
            jumpTimer += Time.deltaTime;
            if(jumpTimer< _jumpCooldown)
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

        }

        else if(!jump && wallJump && _wallJumpAgain)
        {
            _rb.velocity = new Vector2(-_dirX * _wallJumpForce.x, _wallJumpForce.y);
            wallSliding = false;
            _player.StartCoroutine(ReactivateMove());
            _player.StartCoroutine(ReactivateJump());
        }
    }

    public void JumpEvent(InputAction.CallbackContext obj)
    {
        jump = false;
        OnJump(_player.color);
    }

    public void SaveTin(ColorTin tin, int index)
    {
        if (index != 2) _player.inventory[index] = tin;
    }

   
    IEnumerator ReactivateMove()
    {
        yield return new WaitForSeconds(_wallJumpDuration);
        wallJump = false;
    }

    IEnumerator ReactivateJump()
    {
        _wallJumpAgain = false;
        yield return new WaitForSeconds(0.2f);
        _wallJumpAgain = true;
    }

    bool CoyoteTime(Transform transform, float dist , LayerMask floor)
    {
        return Physics2D.Raycast(transform.position, Vector2.down, dist, floor);
    }

}
