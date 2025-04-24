using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR;

public enum EState
{
    Idle,
    Patrol,
    Trace,
    Return,
    Attack,
    Damaged,
    Die
}
public abstract class AStateMachine
{
    protected AStateMachineOwner _owner;
    protected AState _currentState;
    public Dictionary<EState, AState> StateDict = new Dictionary<EState, AState>();

    public abstract void InitializeState();

    public AStateMachine(AStateMachineOwner owner)
    {
        _owner = owner;
    }
    public void Update()
    {
        _currentState.Update();
    }
    public void ChangeState(AState targetState)
    {
        _currentState.Exit();
        targetState.Enter();
    }
}
