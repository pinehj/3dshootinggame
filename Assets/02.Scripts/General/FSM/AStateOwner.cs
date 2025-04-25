using System.Collections.Generic;
using UnityEngine;

public abstract class AStateMachineOwner : MonoBehaviour
{
    protected AStateMachine _statemachine;
    protected Dictionary<EState, AState> _stateDict;

    public abstract void InitializeStateMachine();
    public abstract void InitializeStates();
}
