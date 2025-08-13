using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyCollisions 
{
    FastEnemy _enemy;
    public FastEnemyCollisions(FastEnemy enemy)
    {
        _enemy = enemy;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            _enemy.ChangeAfterJump(true);
        }

        if(collision.gameObject.layer == 6)
        {
            _enemy.ChangeAfterJump(false);
        }
    }
}
