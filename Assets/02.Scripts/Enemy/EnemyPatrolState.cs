using System.Collections;
using UnityEngine;


public class EnemyPatrolState : AState
{
    private Enemy _enemy;
    private float _waitAndPatrolTimer = 0;

    public EnemyPatrolState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _waitAndPatrolTimer = 0;
    }

    public override void Exit()
    {
        _waitAndPatrolTimer = 0;
    }

    public override void Update()
    {
        if(_waitAndPatrolTimer > _enemy.WaitAndPatrolTime)
        {
            Debug.Log("상태전환: Idle -> Patorl");
            _stateMachine.ChangeState(_stateMachine.StateDict[EState.Patrol]);
            return;
        }
       
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            _stateMachine.ChangeState(_stateMachine.StateDict[EState.Trace]);
            return;
        }

        _waitAndPatrolTimer += Time.deltaTime;
    }
}
