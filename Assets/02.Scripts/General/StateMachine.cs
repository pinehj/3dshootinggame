using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR;

public class StateMachine
{
    private AState _currentState;
    private AStateMachineOwner _stateMachineOwner;
    public AStateMachineOwner StateMachineOwner => _stateMachineOwner;

    public StateMachine(AStateMachineOwner stateMachineowner)
    {
        _stateMachineOwner = stateMachineowner;
    }

    private void Update()
    {
        _currentState.Update();
    }
    public void ChangeState(AState targetState)
    {
        _currentState.Exit();
        targetState.Enter();
    }
}
