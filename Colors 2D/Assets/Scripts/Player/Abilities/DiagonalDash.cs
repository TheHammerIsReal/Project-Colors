using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalDash : IAbility
{
    float _dashForce , _dashCooldown, _dashForceY;
    Rigidbody2D _rb;
    PlayerModel _model;
    public DiagonalDash(PlayerModel model , Rigidbody2D rb)
    {
        _model = model;
        _dashForce = model.dashForce;
        _dashCooldown = model.dashCooldown;
        _dashForceY = model.dashForceY;
        _rb = rb;
    }

    public void Ability(Player player)
    {
        if (!_model.isDashing)
        {
            player.StartCoroutine(Dash(player));
            _model.isDashing = true;
            player.ColorWaste();
        }
    }

    IEnumerator Dash(Player player)
    {
        var initialGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(player.transform.right.x * _dashForce, Vector2.up.y * _dashForceY);
        yield return new WaitForSeconds(_dashCooldown);
        //Modify to DoTween
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = initialGravity;
        _model.isDashing = false;
    }
}
