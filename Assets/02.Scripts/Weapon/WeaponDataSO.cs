using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "Scriptable Objects/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    public float Damage;
    public float KnockbackPower;
    public int MagazineCapacity;
    public float RateOfFirePerSec;
    public float ReloadTime;
    public float ZoomMult;
}
