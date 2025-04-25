using UnityEngine;

public class EnemyTypeFollowTraceState : EnemyTraceState
{
    public EnemyTypeFollowTraceState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {
        _enemy = enemy;
    }

    public override void Update()
    {
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) < _enemy.AttackDistance)
        {
            _stateMachine.ChangeState(EState.Attack);
            return;
        }
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);
    }
}
