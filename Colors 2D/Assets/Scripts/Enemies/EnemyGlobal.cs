using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGlobal : MonoBehaviour , IDamageable 
{
    public ColorTone color;

    [SerializeField] protected int _speed;

    protected Vector3 _dir;

    protected Player _player;

    [SerializeField] protected float _lerpRotation;

    [SerializeField] protected float _viewRadius;

    [SerializeField] LayerMask _playerMask = 1 << 10;

    public Node initialNode;

    public void TakeDmg(int dmg)
    {
        Destroy(gameObject);
    }

    protected void FindPlayer(float viewRadius)
    {
        var playerCol = Physics2D.OverlapCircle(transform.position, viewRadius, _playerMask);


        if (playerCol != null)
        {
            var playerComp = playerCol.GetComponent<Player>();

            if (playerComp != null)
            {
                _player = playerComp;
                Debug.Log("Player visto");
            }
        }

        
    }

    public static void TurnOnCallBack(EnemyGlobal enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    public static void TurnOffCallBack(EnemyGlobal enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
    }
}
