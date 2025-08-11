using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Player_Collisions
{
    PlayerModel _model;
    Player _player;
    bool _hasTin = false;
    public ColorTin tin;
    public event Action<ColorTin,int> OnTinSaved = delegate { };
    public event Action<bool> OnLanded = delegate { };
    bool _slideOnWall;
    LayerMask _wallsLayer;
    Collider2D _collider;
    LayerMask _railLayer;
    bool _collisionWall = false;
    int _groundDmg;
    bool _onFloor;
    Rigidbody2D _rb;
    Vector3 _direction;

    public Player_Collisions(PlayerModel model , Player player, Rigidbody2D rb)
    {
        _model = model;
        _player = player;
        _wallsLayer = player.wallsLayer;
        _collider = player.GetComponent<Collider2D>();
        _railLayer = player.railLayer;
        _groundDmg = player.GroundDmg;
        _rb = rb;
    }

    public void OnCollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            _model.jump = true;
            _model.jumpTimer = 0;
            _model.wallJump = false;
            _collisionWall = false;
            _model.wallSliding = false;
            _onFloor = true;
            OnLanded(_slideOnWall);
            if (_model.groundAttack)
            {
                GroundAttackHit();
                _model.groundAttack = false;
            }
        }

        if(collision.gameObject.layer == 7)
        {
            _model.wallJump = _collisionWall;
            _model.wallSliding = _collisionWall;
        }

        if (_model.doubleJump)
        {
            HitAbility(collision);
            _model.doubleJump = false;
        }

        if (_model.isDashing) HitAbility(collision);

    }
    public void OnCollisionExit(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            RailCheck();
        } 
        
        if(collision.gameObject.layer == 7)
        {
            //_model.wallJump = false;
            _collisionWall = false;
            _model.wallSliding = false;
        }

    }

    public void OnTriggerEnter(Collider2D other)
    {
        tin = other.GetComponent<ColorTin>();

        if (tin != null) _hasTin = true;
    }

    public void OnTriggerExit(Collider2D other)
    {
        if (other.GetComponent<ColorTin>() != null)
        {
            tin = null;
            _hasTin = false;

        }
    }

    public void SaveTinEvent(InputAction.CallbackContext obj)
    {
        SaveTin(tin);
    }

    void SaveTin(ColorTin tin)
    {
        if (_hasTin == true)
        {
            OnTinSaved(tin,_player.InventoryIndex());
            _hasTin = false;
        }
    }

    public void CollisionJump(ColorTone color)
    {
        _onFloor = false;
        _player.StartCoroutine(JumpRaycast());
    }

    IEnumerator JumpRaycast()
    {
        while (!_onFloor)
        {
            RailCheck();
            WallCheck();

            yield return null;
        }
    }

    public void RailCheck()
    {
        var hit = Physics2D.Raycast(_player.transform.position, Vector2.down, float.MaxValue, _railLayer);

        if (hit)
        {
            var rail = hit.collider.GetComponent<Rail>();

            if (rail != null)
            {
                if (rail.color != _player.color)
                {
                    Physics2D.IgnoreCollision(_collider, hit.collider);
                    _model.jump = false;
                }

                else Physics2D.IgnoreCollision(_collider, hit.collider, false);
            }

        }
    }

    public void WallCheck()
    {
        if (_rb.velocity.x != 0) _direction = _rb.velocity;
        _direction.y = 0;
        _direction.z = 0;


        var hit = Physics2D.Raycast(_player.transform.position, _direction, float.MaxValue, _wallsLayer);

        if (hit)
        {
            var wall = hit.collider.GetComponent<Walls>();

            if (wall != null)
            {
                if (wall.color != _player.color)
                {
                    Physics2D.IgnoreCollision(_collider, hit.collider);
                    _collisionWall = false;
                }

                else
                {
                    Physics2D.IgnoreCollision(_collider, hit.collider, false);
                    _collisionWall = true;
                }
            }
        }
    }

    void GroundAttackHit()
    {
        var hit = Physics2D.OverlapCircleAll(_player.groundAttackBox.position, _player.groundAttackBox.localScale.x);

        foreach (var item in hit)
        {
            if (item.GetComponent<Player>() != null) continue;

            var dmg = item.GetComponent<IDamageable>();

            if (dmg != null) dmg.TakeDmg(_groundDmg);
        }
    }


    void HitAbility(Collision2D collision)
    {
        var dmg = collision.gameObject.GetComponent<IDamageable>();

        if (dmg != null) dmg.TakeDmg(_groundDmg);

        //en un futuro con el sprite del personaje averiguar como pasarlo de forma optima al collider de los pies o que solo detecte las colisiones que vienen de abajo
        //agregar parametro para identificar el ataque y elegir su parte del sprite correspondiente
        //modificar metodo para que no se frene en el dash
    }
}

       
    

