using UnityEngine;

public abstract class AState
{
    protected StateMachine _stateMachine;

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
