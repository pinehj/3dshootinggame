using UnityEngine;

public class EnemyReturnState : AState
{
    private Enemy _enemy;

    public EnemyReturnState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
        _enemy.Agent.ResetPath();
    }

    public override void Update()
    {
        if (Vector3.Distance(_enemy.transform.position, _enemy.StartPosition) <= _enemy.ReturnDistance)
        {
            _stateMachine.ChangeState(EState.Idle);
            _enemy.Animator.SetTrigger("MoveToIdle");
            return;
        }

        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.FindDistance)
        {
            _stateMachine.ChangeState(EState.Trace);
            return;
        }
        //Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _enemy.Agent.SetDestination(_enemy.StartPosition);
    }
}
