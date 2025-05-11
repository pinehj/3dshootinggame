using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public enum EWeaponType
{
    Gun,
    Throw,
    Melee
}

public abstract class Weapon : MonoBehaviour
{
    [Header("타입")]
    [SerializeField] protected EWeaponType _weaponType;
    public EWeaponType WeaponType => _weaponType;
    [Header("무기 설정값")]
    public WeaponDataSO WeaponData;
    public Transform WeaponTransform;
    [Header("무기 상태")]
    [SerializeField] protected bool _isEquiped;
    [SerializeField] protected bool _isReloading;

    [SerializeField] protected int _currentMagazine;
    public int CurrentMagazine
    {
        get
        {
            return _currentMagazine;
        }
        protected set
        {
            _currentMagazine = value;
            if (!_isEquiped)
            {
                return;
            }
            UIManager.Instance.UpdateBulletCount(_currentMagazine, WeaponData.MagazineCapacity);
        }
    }

    [Header("공격")]
    [SerializeField] protected float _attackTimer;


    public virtual void Initialize()
    {
        CurrentMagazine = WeaponData.MagazineCapacity;
    }
    protected virtual void Update()
    {
        _attackTimer -= Time.deltaTime;
    }

    public virtual void Equip()
    {
        gameObject.SetActive(true);
        _isEquiped = true;
    }
    public virtual void Unequip()
    {
        gameObject.SetActive(false);
        _isEquiped = false;
    }
    public virtual bool Primary()
    {
        if (_attackTimer <= 0 && CurrentMagazine > 0)
        {
            _attackTimer = 1 / WeaponData.RateOfFirePerSec;
            return true;
        }
        else
        {
            return false;
        }
    }
    public abstract void Alternate();
    public abstract void PerformAttack();
    public abstract void StopAttack();
    public abstract void Reload();
}
