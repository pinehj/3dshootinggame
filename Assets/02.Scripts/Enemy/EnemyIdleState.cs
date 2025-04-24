using System.Collections;
using UnityEngine;


public class EnemyIdleState : AState
{
    private float _waitAndPatrolTimer = 0;
    public override void Enter()
    {
        _waitAndPatrolTimer = 0;
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        _waitAndPatrolTimer += Time.deltaTime;
    }
}
