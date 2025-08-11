using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2;

public class EnemyShooter : EnemyGlobal
{
    EventFSM<ShooterStates> _stateMachine;
    EnemyShooterView _view;
    [SerializeField] float _shootRate;
    float _shootTimer;
    [SerializeField] float _minDistPatrolPoints;
    public List<Node> patrolPoints;
    [SerializeField] Transform _spawnPointShoot;
    int _indexPatrol;

    private void Awake()
    {
        _view = new EnemyShooterView(this);

        #region StateCreator

        var patrol = new State<ShooterStates>("Patrol");
        var attack = new State<ShooterStates>("Attack");

        #endregion

        #region Transitions

        StateConfigurer.Create(patrol)
                        .SetTransition(ShooterStates.Attack, attack)
                        .Done();

        StateConfigurer.Create(attack)
                       .SetTransition(ShooterStates.Patrol, patrol)
                       .Done();

        #endregion

        #region Patrol

        patrol.OnUpdate += () =>
        {
            FindPlayer(_viewRadius);
            if (_player != null) ChangeState(ShooterStates.Attack);
            Patrol(patrolPoints , _minDistPatrolPoints , ref _dir , transform);
        };

        patrol.OnFixedUpdate += Move;



        #endregion

        #region Attack

        attack.OnUpdate += () =>
         {
             _view.Rotation(_player.transform, _lerpRotation);

             _shootTimer += Time.deltaTime;

             if(_shootTimer>= _shootRate)
             {
                 Shoot();
                 _shootTimer = 0;
             }
         };

        attack.OnExit += x =>
         {
             _shootTimer = 0;
         };


        #endregion

        _stateMachine = new EventFSM<ShooterStates>(patrol);

    }

    void ChangeState(ShooterStates state) => _stateMachine.SendInput(state);

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _dir, _speed * Time.fixedDeltaTime);
    } 

    void Shoot()
    {
        var bullet = SpawneableFactory.instance.GetObjFromPool(SpawnType.EnemyBullet);
        bullet.SetValues(_spawnPointShoot.position, transform.right);
    }

    void Patrol(List<Node> patrolPoints , float minDist , ref Vector3 dir , Transform transform)
    {
        dir = patrolPoints[_indexPatrol].transform.position;

        transform.right = dir;

        var dist = dir - transform.position;
        
        if(dist.sqrMagnitude <= minDist * minDist)
        {
            _indexPatrol++;

            if (_indexPatrol >= patrolPoints.Count) _indexPatrol = 0;
        }
    }
}

public enum ShooterStates
{
    Patrol,
    Attack
}
