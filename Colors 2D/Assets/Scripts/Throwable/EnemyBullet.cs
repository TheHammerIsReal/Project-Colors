using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Spawneable
{
    Vector3 _dir;
    [SerializeField] int _speed;
    [SerializeField] int _dmg;
    
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
        var dmg = collision.gameObject.GetComponent<IDamageable>();

        if (dmg != null) dmg.TakeDmg(_dmg);

        SpawneableFactory.instance.ReturnToPool(this, SpawnType.EnemyBullet);
    }

    public override void SetValues(params object[] parameters)
    {
        Vector3 pos = (Vector3)parameters[0];
        Vector3 dir = (Vector3)parameters[1];

        transform.position = pos;
        _dir = dir;
    }
}
