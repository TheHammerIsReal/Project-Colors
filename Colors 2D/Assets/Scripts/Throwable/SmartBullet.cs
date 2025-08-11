using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBullet : Spawneable
{
    [SerializeField] int _speed;
    Vector3 _dir;
    [SerializeField] float _spawnAdjustment;

    private void FixedUpdate()
    {
        transform.position += _dir * _speed * Time.fixedDeltaTime;
    }

    public static void TurnOnCallBack(Spawneable bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    public static void TurnOffCallBack(Spawneable bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyGlobal>();

        if (enemy != null)
        {
            //Congelar enemigo o lo que sea
            return;
        }
    }

    public override void SetValues(params object[] parameters)
    {
        Vector3 pos = (Vector3)parameters[0];
        Vector3 dir = (Vector3)parameters[1];

        transform.position = pos;
        _dir = dir;
    }
}
