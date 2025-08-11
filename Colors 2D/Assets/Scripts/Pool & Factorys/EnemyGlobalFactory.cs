using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalFactory : MonoBehaviour
{
    public static EnemyGlobalFactory instance { get; private set; }
    Pool<EnemyGlobal> _enemyPool;
    [SerializeField] EnemyGlobal _enemyPrefab;
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

        _enemyPool = new Pool<EnemyGlobal>(CreatorMethod, EnemyGlobal.TurnOnCallBack, EnemyGlobal.TurnOffCallBack, _initialAmount);
    }

    EnemyGlobal CreatorMethod()
    {
        return Instantiate(_enemyPrefab);
    }

    public EnemyGlobal GetObjFromPool()
    {
        return _enemyPool.GetObj();
    }

    public void ReturnToPool(EnemyGlobal obj)
    {
        _enemyPool.Return(obj);
    }

    public enum EnemyType
    {
        Shooter
    }
}
