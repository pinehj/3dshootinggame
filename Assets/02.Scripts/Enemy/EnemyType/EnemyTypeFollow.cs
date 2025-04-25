using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeFollow : Enemy
{
    protected override void Start()
    {
        base.Start();
        Type = EEnemyType.Follow;
    }
    public override void InitializeStates()
    {
        _stateDict = new Dictionary<EState, AState>
        {
            { EState.Trace, new EnemyTypeFollowTraceState(_statemachine, this) },
            { EState.Attack, new EnemyAttackState(_statemachine, this) },
            { EState.Return, new EnemyReturnState(_statemachine, this) },
            { EState.Damaged, new EnemyDamagedState(_statemachine, this) },
            { EState.Die, new EnemyDieState(_statemachine, this) },
        };
    }
    public override void InitializeStateMachine()
    {
        _statemachine = new EnemyStateMachine(this);
        InitializeStates();
        _statemachine.InitializeDict(_stateDict, EState.Trace);
    }

}
