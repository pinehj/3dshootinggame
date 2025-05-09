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
    private bool _isThrowed;
    protected virtual void LateUpdate()
    {
        transform.position = WeaponTransform.position;
        transform.rotation = WeaponTransform.rotation;

        if(_bombProjectile != null && !_isThrowed)
        {
            _bombProjectile.transform.position = WeaponTransform.position;
            _bombProjectile.transform.rotation = WeaponTransform.rotation;
            Debug.Log("ss");
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
        _isThrowed = false;

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
        _isThrowed = true;
        _bombProjectile.IsThrowed = true;
        CurrentMagazine--;
        Rigidbody projectileRigidbody = _bombProjectile.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce((Camera.main.transform.forward + Camera.main.transform.up / 2) * ThrowBombPower
                                //* (ThrowBombPower + BombChargePower * (BombChargeTime / PlayerFireDataSO.MaxBombChargeTime))
                                , ForceMode.Impulse);
        projectileRigidbody.AddTorque(Vector3.one);


        _bombProjectile = null;
        _isThrowed = false;

        if (CurrentMagazine > 0)
        {
            Equip();
        }
    }
}
