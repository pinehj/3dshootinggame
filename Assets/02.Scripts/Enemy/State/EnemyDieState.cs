using System.Collections;
using UnityEngine;


public class EnemyDieState : AState
{
    private Enemy _enemy;
    private float _dieTimer = 0;

    public EnemyDieState(AStateMachine stateMachine, Enemy enemy) : base(stateMachine)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _dieTimer = 0;
    }

    public override void Exit()
    {
        _dieTimer = 0;
    }

    public override void Update()
    {
        if(_dieTimer > _enemy.DieTime)
        {
            Debug.Log("Die");
            _enemy.gameObject.SetActive(false);
            return;
        }
    }
}
