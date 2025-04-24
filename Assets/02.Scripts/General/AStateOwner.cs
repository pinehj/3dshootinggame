using UnityEngine;

public abstract class AStateMachineOwner : MonoBehaviour
{
    protected AStateMachine _statemachine;

    public abstract void InitializeStateMachine();
}
