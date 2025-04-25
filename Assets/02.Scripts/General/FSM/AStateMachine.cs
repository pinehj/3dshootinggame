using System.Collections.Generic;
using UnityEngine;

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
    protected Dictionary<EState, AState> _stateDict;

    public AStateMachine(AStateMachineOwner owner)
    {
        _owner = owner;
    }

    public virtual void InitializeDict(Dictionary<EState, AState> stateDict , EState startState)
    {
        _stateDict = stateDict;
        ChangeState(startState);
    }

    public void Update()
    {
        _currentState.Update();
    }
    public void ChangeState(EState targetState)
    {

        if (_currentState != null)
        {
            if (_currentState == _stateDict[targetState])
            {
                return;
            }

            Debug.Log($"State Changed {_currentState} -> {targetState}");
            _currentState.Exit();
        }

        _currentState = _stateDict[targetState];
        _currentState.Enter();
    }
}
