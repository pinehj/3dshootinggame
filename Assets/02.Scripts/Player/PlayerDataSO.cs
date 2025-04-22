using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    public float WalkSpeed;
    public float RunSpeed;

    public float MaxStamina;
    public float RunStaminaCost;
    public float DashStaminaCost;
    public float DashDuration;
    public float ClimbStaminaCost;
    public float StaminaRegen;
    public float JumpPower;
    public int MaxJumpCount;
    public float DashPower;
    public float ClimbSpeed;
}
