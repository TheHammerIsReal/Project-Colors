using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack :IAbility
{
    Rigidbody2D _rb;
    Transform _player;
    PlayerModel _model;

    public GroundAttack(Rigidbody2D rb , PlayerModel model)
    {
        _rb = rb;
        _model = model;
    }

    public void Ability(Player player)
    {
        if (!_model.jump)
        {
            _model.groundAttack = true;
            player.StartCoroutine(GroundCoroutine(player.GravityScaleGround));
            player.ColorWaste();
        }
        
    }

    IEnumerator GroundCoroutine(int gravityScaleGround)
    {
        WaitUntil jump = new WaitUntil(() => _model.jump);

        var initialScale = _rb.gravityScale;

        _rb.gravityScale = gravityScaleGround;

        yield return jump;

        _rb.gravityScale = initialScale;
    }
   
}
