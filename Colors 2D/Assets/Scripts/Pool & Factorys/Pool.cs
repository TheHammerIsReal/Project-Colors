using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pool<T>
{
    Func<T> _factoryMethod;

    List<T> _currentStock;

    Action<T> _turnOnCallBack;
    Action<T> _turnOffCallBack;

    public Pool(Func<T> factoryMethod ,Action<T> turnOnCallBack, Action<T> turnOffCallBack, int initialAmount)
    {
        _currentStock = new List<T>();
        
        _factoryMethod = factoryMethod;

        _turnOnCallBack = turnOnCallBack;
        
        _turnOffCallBack = turnOffCallBack;

        for (int i = 0; i < initialAmount; i++)
        {
            T newObj = _factoryMethod();
            _turnOffCallBack(newObj);
            _currentStock.Add(newObj);

        }
    }
    
    public T GetObj()
    {
        T obj;
        if (_currentStock.Count==0)
        {
            obj = _factoryMethod();
        }

        else
        {
            obj = _currentStock[0];
            _currentStock.RemoveAt(0);
        }

        _turnOnCallBack(obj);

        return obj;
    }

    public void Return(T obj)
    {
        _turnOffCallBack(obj);

        _currentStock.Add(obj);
    }
}
