using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Spawneable
{
    [SerializeField] int _speed;
    Vector3 _dir;
    [SerializeField] int _dmg;
    [SerializeField] Transform _enemyCheck;
    [SerializeField] Collider2D _collider;
    [SerializeField] LayerMask _enemiesLayer;

    void Update()
    {
        Raycast(_enemyCheck, _collider);
    }

    private void FixedUpdate()
    {
        transform.position += _dir * _speed * Time.fixedDeltaTime;
    }

    void Raycast(Transform enemyCheck , Collider2D collider)
    {
        var hit = Physics2D.OverlapBox(enemyCheck.position, enemyCheck.localScale, 0 , _enemiesLayer);

        if (hit != null)
        {
            var enemy = hit.GetComponent<EnemyGlobal>();

            if (enemy.color == ColorTone.Red || enemy.color == ColorTone.Violet || enemy.color == ColorTone.Orange)
                Physics2D.IgnoreCollision(collider, hit);

            else Physics2D.IgnoreCollision(collider, hit, false);
        }
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

        if (dmg != null)
        {
            dmg.TakeDmg(_dmg);
            SpawneableFactory.instance.ReturnToPool(this, SpawnType.Bullet); 
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
