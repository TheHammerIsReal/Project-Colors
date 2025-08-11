using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawneable : MonoBehaviour
{
    public abstract void SetValues(params object[] parameters);

    public SpawnType type;
}

public enum SpawnType
{
    Bullet,
    EnemyBullet,
    SmartBullet
}
