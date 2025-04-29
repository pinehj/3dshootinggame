using UnityEngine;

public class EnemyIdleState : AState
{
    private Enemy _enemy;
    private float _waitAndPatrolTimer = 0;

    public EnemyIdleState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
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
            _stateMachine.ChangeState(EState.Patrol);
            _enemy.Animator.SetTrigger("IdleToMove");
            return;
        }
       
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.FindDistance)
        {
            _stateMachine.ChangeState(EState.Trace);
            _enemy.Animator.SetTrigger("IdleToMove");
            return;
        }

        _waitAndPatrolTimer += Time.deltaTime;
    }
}
