using UnityEngine;

public class EnemyDamagedState : AState
{
    private Enemy _enemy;
    private float _damagedTimer;
    public EnemyDamagedState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _damagedTimer = 0;
        _enemy.Agent.speed = _enemy.KnockbackedPower;
    }

    public override void Exit()
    {
        _damagedTimer = 0;
        _enemy.Agent.speed = _enemy.MoveSpeed;
    }

    public override void Update()
    {
        if (_damagedTimer > _enemy.DamageTime)
        {
            _stateMachine.ChangeState(EState.Trace);
            return;
        }
        Vector3 dir = (_enemy.transform.position - _enemy.DamagedOrigin).normalized;

        _enemy.Agent.SetDestination(_enemy.transform.position + dir);
        _damagedTimer += Time.deltaTime;
    }
}
