using UnityEngine;

public class EnemyAttackState : AState
{
    private Enemy _enemy;
    private float _attackTimer = 0;

    public EnemyAttackState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _attackTimer = 0;
    }

    public override void Exit()
    {
        _attackTimer = 0;
    }

    public override void Update()
    {
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) > _enemy.AttackDistance)
        {
            _stateMachine.ChangeState(EState.Trace);
            _enemy.Animator.SetTrigger("AttackDelayToMove");
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _enemy.AttackCoolTime)
        {
            _enemy.Animator.SetTrigger("AttackDelayToAttack");
            _attackTimer = 0;
        }

        _attackTimer += Time.deltaTime;
    }
}
