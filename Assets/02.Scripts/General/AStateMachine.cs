using System.Collections.Generic;

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
    protected Dictionary<EState, AState> _stateDict = new Dictionary<EState, AState>();

    public abstract void InitializeState();

    public AStateMachine(AStateMachineOwner owner)
    {
        _owner = owner;
    }
    public void Update()
    {
        _currentState.Update();
    }
    public void ChangeState(EState targetState)
    {
        
        if(_currentState != null){
            if(_currentState == _stateDict[targetState])
            {
                return;
            }
            _currentState.Exit();
        }
        _currentState = _stateDict[targetState];
        _currentState.Enter();
    }
}
