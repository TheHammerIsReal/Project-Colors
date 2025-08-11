using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterView
{
    EnemyShooter _enemyShooter;
    
    public EnemyShooterView(EnemyShooter enemyShooter)
    {
        _enemyShooter = enemyShooter;
    }

    public void Rotation(Transform playerTransform , float lerpRotation)
    {
        //ajustar rotacion solamente eje z

        var playerPos = playerTransform.position - _enemyShooter.transform.position;

        var angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;

        var rotation = Quaternion.Euler(0, 0, angle);

        _enemyShooter.transform.rotation = Quaternion.Lerp(_enemyShooter.transform.rotation, rotation, lerpRotation * Time.deltaTime);
    }
}
