using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawneableFactory : MonoBehaviour
{
    public static SpawneableFactory instance { get; private set; }
    Pool<Spawneable> _bulletPool;
    Pool<Spawneable> _enemyBulletPool;
    Pool<Spawneable> _smartBulletPool;
    [SerializeField] Spawneable _bullet;
    [SerializeField] Spawneable _enemyBullet;
    [SerializeField] Spawneable _smartBullet;
    [SerializeField] int _initialAmount;
    Dictionary<SpawnType, Pool<Spawneable>> _dictionary = new Dictionary<SpawnType, Pool<Spawneable>>();

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        else
        {
            instance = this;
        }

        _bulletPool = new Pool<Spawneable>(BulletCreatorMethod, Bullet.TurnOnCallBack, Bullet.TurnOffCallBack, _initialAmount);
        _enemyBulletPool = new Pool<Spawneable>(EnemyBulletCreatorMethod, EnemyBullet.TurnOnCallBack, EnemyBullet.TurnOffCallBack, _initialAmount);
        _smartBulletPool = new Pool<Spawneable>(SmartBulletCreatorMethod, SmartBullet.TurnOnCallBack, SmartBullet.TurnOffCallBack, _initialAmount);

        _dictionary.Add(SpawnType.Bullet, _bulletPool);
        _dictionary.Add(SpawnType.EnemyBullet, _enemyBulletPool);
        _dictionary.Add(SpawnType.SmartBullet, _smartBulletPool);
    }

    Spawneable BulletCreatorMethod()
    {
        return Instantiate(_bullet);
    }

    Spawneable EnemyBulletCreatorMethod()
    {
        return Instantiate(_enemyBullet);
    }

    Spawneable SmartBulletCreatorMethod()
    {
        return Instantiate(_smartBullet);
    }

    public Spawneable GetObjFromPool(SpawnType type)
    {
        return _dictionary[type].GetObj();
    }

    public void ReturnToPool(Spawneable obj, SpawnType type)
    {
        _dictionary[type].Return(obj);
    }
}
