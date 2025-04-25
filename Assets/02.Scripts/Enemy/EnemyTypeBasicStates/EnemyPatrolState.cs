using UnityEngine;

public class EnemyPatrolState : AState
{
    private Enemy _enemy;
    private int _patrolIndex = 0;

    public EnemyPatrolState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _patrolIndex = 0;
    }

    public override void Exit()
    {
        _enemy.Agent.ResetPath();
        _patrolIndex = 0;
    }

    public override void Update()
    {
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.FindDistance)
        {
            _stateMachine.ChangeState(EState.Trace);
            return;
        }
        if (Vector3.Distance(_enemy.transform.position, _enemy.PatrolPointPositions[_patrolIndex]) <= _enemy.ReturnDistance)
        {
            _patrolIndex = (_patrolIndex + 1) % _enemy.PatrolPointsNum;
            return;
        }
        _enemy.Agent.SetDestination(_enemy.PatrolPointPositions[_patrolIndex]);
    }
}
