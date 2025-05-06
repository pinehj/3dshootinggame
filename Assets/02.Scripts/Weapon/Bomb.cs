using Unity.VisualScripting;
using UnityEngine;

public class Bomb : Weapon
{
    //목표: 마우스가 오른쪽 버튼을 누르면 카메라가 
    [Header("폭탄")]
    [SerializeField] private int _bombCount;
    public int BombCount
    {
        get
        {
            return _bombCount;
        }
        set
        {
            _bombCount = Mathf.Clamp(value, 0, WeaponData.MagazineCapacity);
            UIManager.Instance.UpdateBombCount(_bombCount, WeaponData.MagazineCapacity);
        }
    }
    [SerializeField] private float _bombChargeTime;
    public GameObject FireBombPosition;
    public GameObject BombPrefab;
    public float ThrowBombPower;

    public override void Alternate()
    {
    }

    public void Initialize()
    {
    }

    public override void PerformAttack()
    {
        BombCount--;
        BombProjectile bomb = BombPool.Instance.GetFromPool(FireBombPosition.transform.position);

        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        bombRigidbody.AddForce((Camera.main.transform.forward + Camera.main.transform.up / 2) * ThrowBombPower
                                //* (ThrowBombPower + BombChargePower * (BombChargeTime / PlayerFireDataSO.MaxBombChargeTime))
                                , ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);

        //BombChargeTime = 0;
    }

    public override void Reload()
    {
    }

    public override void StopAttack()
    {
    }


}
