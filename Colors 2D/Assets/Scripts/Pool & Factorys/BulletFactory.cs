using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory instance { get; private set; }
    Pool<Bullet> _bulletPool;
    [SerializeField] Bullet _bulletPrefab;
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

        _bulletPool = new Pool<Bullet>(CreatorMethod, Bullet.TurnOnCallBack, Bullet.TurnOffCallBack, _initialAmount);
    }

    Bullet CreatorMethod()
    {
        return Instantiate(_bulletPrefab);
    }

    public Bullet GetObjFromPool()
    {
        return _bulletPool.GetObj();
    }
     
    public void ReturnToPool(Bullet obj)
    {
        _bulletPool.Return(obj);
    }
}
