using System.Collections;
using UnityEngine;

public class HorizontalDash : IAbility
{
    float _dashForce;
    Rigidbody2D _rb;
    PlayerModel _model;
    float _dashCooldown;

    public HorizontalDash(Rigidbody2D rb, PlayerModel model)
    {
        _rb = rb;
        _model = model;
        _dashForce = model.dashForce;
        _dashCooldown = model.dashCooldown;
    }

    public void Ability(Player player)
    {
        if (!_model.isDashing)
        {
            player.StartCoroutine(Dashing(player));
            _model.isDashing = true;
            player.ColorWaste();
            //Llamar efectos particulas etc.
        }
        
    }

    IEnumerator Dashing(Player player)
    {
        var initialGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(player.transform.right.x * _dashForce,0f);
        //Logica daño mientras dashea.
        yield return new WaitForSeconds(_dashCooldown);
        _rb.velocity = Vector2.zero;
        _rb.gravityScale = initialGravity;
        _model.isDashing = false;
       
    }
}
