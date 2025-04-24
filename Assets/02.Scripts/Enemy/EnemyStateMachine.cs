using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine : AStateMachine
{
    private Enemy _enemy;
    public EnemyStateMachine(AStateMachineOwner owner) : base(owner)
    {
        //상태에 관한 컴포넌트
        //움직임에 관한 컴포넌트
        _enemy = _owner.GetComponent<Enemy>();
    }

    public override void InitializeState()
    {
        StateDict = new Dictionary<EState, AState>
        {
            { EState.Idle, new EnemyIdleState(this, _enemy) },
            { EState.Patrol, new EnemyPatrolState(this, _enemy) },
            { EState.Trace, new EnemyTraceState(this, _enemy) },
            { EState.Attack, new EnemyAttackState(this, _enemy) },
            { EState.Return, new EnemyReturnState(this, _enemy) },
            { EState.Damaged, new EnemyDamagedState(this, _enemy) },
            { EState.Die, new EnemyDieState(this, _enemy) },
        };
    }
}
