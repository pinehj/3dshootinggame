using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFireDataSO", menuName = "Scriptable Objects/PlayerFireDataSO")]
public class PlayerFireDataSO : ScriptableObject
{
    public float ThrowBombPower;
    public float MaxBombChargeTime;
    public float MaxBombChargePower;
    public int MaxBombCount;
    public int MaxBulletCount;
    public float BulletFireInterval;
    public float BulletReloadTime;
}
