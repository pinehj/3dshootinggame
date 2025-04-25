using UnityEngine;

public abstract class AState
{
    protected AStateMachine _stateMachine;

    public AState(AStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
