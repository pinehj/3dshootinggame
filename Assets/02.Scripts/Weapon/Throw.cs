using Unity.VisualScripting;
using UnityEngine;

public class Throw : Weapon
{
    //목표: 마우스가 오른쪽 버튼을 누르면 카메라가 
    [Header("폭탄")]
    [SerializeField] private int _throwCount;
    public int ThrowCount
    {
        get
        {
            return _throwCount;
        }
        set
        {
            _throwCount = Mathf.Clamp(value, 0, WeaponData.MagazineCapacity);
            UIManager.Instance.UpdateBombCount(_throwCount, WeaponData.MagazineCapacity);
        }
    }
    [SerializeField] private float _throwChargeTime;
    public float ThrowPower;
    [SerializeField] private GrenadeProjectile _projectile;

    protected virtual void LateUpdate()
    {
        transform.position = WeaponTransform.position;

        if(_projectile != null && !_projectile.IsThrowed)
        {
            _projectile.transform.position = WeaponTransform.position;
            _projectile.transform.rotation = WeaponTransform.rotation;
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
            _projectile = GrenadePool.Instance.GetFromPool(transform.position);
            _projectile.transform.rotation = transform.rotation;
        }
    }

    public override void Unequip()
    {
        base.Unequip();

        if(_projectile != null)
        {
            GrenadePool.Instance.ReturnToPool(_projectile);
            _projectile = null;
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
        _projectile.IsThrowed = true;
        CurrentMagazine--;
        Rigidbody projectileRigidbody = _projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = false;
        projectileRigidbody.AddForce(Camera.main.transform.forward * ThrowPower
                                //* (ThrowBombPower + BombChargePower * (BombChargeTime / PlayerFireDataSO.MaxBombChargeTime))
                                , ForceMode.Impulse);
        projectileRigidbody.AddTorque(Vector3.one);


        _projectile = null;
    }
}
