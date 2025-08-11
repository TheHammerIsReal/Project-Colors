using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondJump : IAbility
{
    Rigidbody2D _rb;
    PlayerModel _model;

    public SecondJump(Rigidbody2D rb , PlayerModel model)
    {
        _rb = rb;
        _model = model;
    }

    public void Ability(Player player)
    {
        _rb.velocity = new Vector2(0,Vector2.up.y * player.SecondJumpForce);
        player.ColorWaste();
        _model.doubleJump = true;
        // Llamar metodo efectos, particulas, etc.
    }
}
