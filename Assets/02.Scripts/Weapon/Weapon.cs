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
    [Header("무기 설정값")]
    public WeaponDataSO WeaponData;
    public Transform WeaponPivot;
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


    protected virtual void Start()
    {
        CurrentMagazine = WeaponData.MagazineCapacity;
    }
    protected virtual void Update()
    {
        _attackTimer -= Time.deltaTime;
    }
    protected virtual void LateUpdate()
    {
        float length = Vector3.Distance(transform.position, WeaponPivot.transform.position);
        WeaponPivot.eulerAngles = Camera.main.transform.eulerAngles;
        transform.position = WeaponPivot.transform.position + WeaponPivot.forward * length;
        transform.forward = WeaponPivot.forward;
    }
    public virtual void Equip()
    {
        _isEquiped = true;
    }
    public virtual void Unequip()
    {
        _isEquiped = false;
    }
    public virtual bool Primary()
    {
        if (_attackTimer <= 0 && CurrentMagazine > 0)
        {
            PerformAttack();
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
