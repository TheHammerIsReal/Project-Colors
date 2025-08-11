using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBulletFactory : MonoBehaviour
{
    public static SmartBulletFactory instance { get; private set; }
    Pool<SmartBullet> _smartBulletPool;
    [SerializeField] SmartBullet _smartBulletPrefab;
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

        _smartBulletPool = new Pool<SmartBullet>(CreatorMethod, SmartBullet.TurnOnCallBack, SmartBullet.TurnOffCallBack, _initialAmount);
    }

    SmartBullet CreatorMethod()
    {
        return Instantiate(_smartBulletPrefab);
    }

    public SmartBullet GetObjFromPool()
    {
        return _smartBulletPool.GetObj();
    }

    public void ReturnToPool(SmartBullet obj)
    {
        _smartBulletPool.Return(obj);
    }
}
