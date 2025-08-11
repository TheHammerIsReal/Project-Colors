using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Node> _nodesEnemy1;
    [SerializeField] int _enemiesAmount;
    [SerializeField] List<Transform> _spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies(_enemiesAmount);
    }

    void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = EnemyGlobalFactory.instance.GetObjFromPool();
            enemy.transform.position = _spawnPoints[i].position;
            enemy.initialNode = _nodesEnemy1[0];
            if (enemy is EnemyShooter shooter) shooter.patrolPoints = _nodesEnemy1;
        }
    }
}
