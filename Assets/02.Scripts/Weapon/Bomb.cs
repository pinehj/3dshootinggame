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
    public float ThrowBombPower;

    [SerializeField] private BombProjectile _bombProjectile;

    protected virtual void LateUpdate()
    {
        transform.position = WeaponTransform.position;
        //transform.rotation = WeaponTransform.rotation;

        if(_bombProjectile != null && !_bombProjectile.IsThrowed)
        {
            _bombProjectile.transform.position = WeaponTransform.position;
            _bombProjectile.transform.rotation = WeaponTransform.rotation;
            _bombProjectile.transform.rotation = Quaternion.identity;
        }
    }

    public override void Initialize()
    {
        _weaponType = EWeaponType.Throw;
        base.Initialize();
    }
    public override void Equip()
    {
        gameObject.SetActive(true);

        if (CurrentMagazine > 0)
        {
            _bombProjectile = BombPool.Instance.GetFromPool(transform.position);
            _bombProjectile.transform.rotation = transform.rotation;
        }
    }

    public override void Unequip()
    {
        base.Unequip();

        if(_bombProjectile != null)
        {
            BombPool.Instance.ReturnToPool(_bombProjectile);
            _bombProjectile = null;
        }
        
    }
    public override void Alternate()
    {
    }

    public override void Reload()
    {
    }

    public override void StopAttack()
    {
    }

    public override void PerformAttack()
    {
        _bombProjectile.IsThrowed = true;
        CurrentMagazine--;
        Rigidbody projectileRigidbody = _bombProjectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = false;
        projectileRigidbody.AddForce(Camera.main.transform.forward * ThrowBombPower
                                //* (ThrowBombPower + BombChargePower * (BombChargeTime / PlayerFireDataSO.MaxBombChargeTime))
                                , ForceMode.Impulse);
        //projectileRigidbody.AddTorque(Vector3.one);


        _bombProjectile = null;
    }
}
