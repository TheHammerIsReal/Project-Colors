using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletFactory : MonoBehaviour
{
    public static EnemyBulletFactory instance { get; private set; }
    Pool<EnemyBullet> _bulletPool;
    [SerializeField] EnemyBullet _bulletPrefab;
    [SerializeField] int _initialAmount;

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

        _bulletPool = new Pool<EnemyBullet>(CreatorMethod, EnemyBullet.TurnOnCallBack, EnemyBullet.TurnOffCallBack, _initialAmount);
    }

    EnemyBullet CreatorMethod()
    {
        return Instantiate(_bulletPrefab);
    }

    public EnemyBullet GetObjFromPool()
    {
        return _bulletPool.GetObj();
    }

    public void ReturnToPool(EnemyBullet obj)
    {
        _bulletPool.Return(obj);
    }
}
