using UnityEngine;

public class EnemyTraceState : AState
{
    protected Enemy _enemy;

    public EnemyTraceState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
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
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) > _enemy.FindDistance)
        {
            _stateMachine.ChangeState(EState.Return);
            return;
        }
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.AttackDistance)
        {
            _stateMachine.ChangeState(EState.Attack);
            _enemy.Animator.SetTrigger("MoveToAttackDelay");
            return;
        }
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);
    }
}
