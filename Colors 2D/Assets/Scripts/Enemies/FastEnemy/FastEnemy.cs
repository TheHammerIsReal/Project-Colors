using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2;

public class FastEnemy : EnemyGlobal
{
    FastEnemyView _view;
    EventFSM<FastStates> _stateMachine;
    [SerializeField] float _attackCooldown;
    float _attackTimer;
    Vector3 _jumpVelocity;
    Node _currentDestiny;
    [SerializeField] float _minDist = 0.25f;
    Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _view = new FastEnemyView(this);

        #region State Creator

        var patrol = new State<FastStates>("Patrol");
        var calculate = new State<FastStates>("Calculate");
        var attack = new State<FastStates>("Attack");
        var recalculate = new State<FastStates>("Recalculate");

        #endregion

        #region Transitions
        StateConfigurer.Create(patrol).SetTransition(FastStates.Calculate, calculate).Done();

        StateConfigurer.Create(calculate).SetTransition(FastStates.Attack, attack).Done();

        StateConfigurer.Create(attack).SetTransition(FastStates.Recalculate, recalculate).Done();

        StateConfigurer.Create(recalculate).SetTransition(FastStates.Patrol, patrol).Done();
        #endregion

        #region Patrol

        patrol.OnEnter += x =>
        {
            _currentDestiny = initialNode.neighbour;
            _dir = Patrol(transform, _currentDestiny);
        };

        patrol.OnUpdate += () =>
        {
            var distance = _currentDestiny.transform.position - transform.position;

            if(distance.sqrMagnitude<= _minDist* _minDist)
            {
                _currentDestiny = initialNode;
                _dir = Patrol(transform, _currentDestiny);
            }

            FindPlayer(_viewRadius);
            if (_player != null) ChangeState(FastStates.Calculate);
        };

        patrol.OnFixedUpdate += Move;
        #endregion

        #region Calculate

        calculate.OnEnter += x =>
        {
            CalculateJump(transform, _player);
        };

        calculate.OnUpdate += () =>
        {
            _attackTimer += Time.deltaTime;

            if(_attackTimer >= _attackCooldown)
            {
                ChangeState(FastStates.Attack);
                _attackTimer = 0;
            }
        };

        #endregion

        #region Attack 

        attack.OnEnter += x =>
        {
            Jump();
        };

        #endregion

        _stateMachine = new EventFSM<FastStates>(patrol);
    }

    void ChangeState(FastStates state) => _stateMachine.SendInput(state);

    void Update()
    {
        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _dir, _speed * Time.fixedDeltaTime);
    }

    Vector3 Patrol(Transform transform, Node destiny)
    {
        //Metodo patrol 2 waypoints
        return destiny.transform.position - transform.position;
    }

    void CalculateJump(Transform transform, Player player)
    {
        //Metodo calcular salto
    }

    void Jump()
    {
        //Metodo salto
    }

    public enum FastStates
    {
        Patrol,
        Calculate,
        Attack,
        Recalculate
    }
}
