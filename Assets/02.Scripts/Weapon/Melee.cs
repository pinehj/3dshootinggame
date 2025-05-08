using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Melee : Weapon
{
    [Header("근거리무기 설정값")]
    public Transform CastPoint;
    [SerializeField] private float _castRad;
    [SerializeField] private float _castAngleRange;
    [SerializeField] private LayerMask _targetLayer;

    public override void Alternate()
    {
    }

    public override void PerformAttack()
    {
        
        Collider[] hits = Physics.OverlapSphere(CastPoint.position, _castRad, _targetLayer);

        foreach(Collider hit in hits)
        {
            if (IsInAttackRange(hit.transform.position))
            {
                IDamageable damagedEntity = hit.GetComponent<IDamageable>();
                if (damagedEntity != null)
                {
                    Damage damage = new Damage();
                    damage.Value = 10;
                    damage.KnockbackPower = 10;
                    damage.From = gameObject;
                    damagedEntity.TakeDamage(damage);
                }
            }
        }

        _attackTimer = 1 / WeaponData.RateOfFirePerSec;
    }

    public bool IsInAttackRange(Vector3 targetPos)
    {
        float dot = Vector3.Dot((targetPos - CastPoint.position).normalized, CastPoint.forward);
        float theata = Mathf.Acos(dot);

        float degree = Mathf.Rad2Deg * theata;

        return degree <= _castAngleRange / 2;
    }
    public override void Reload()
    {
    }

    public override void StopAttack()
    {
    }
}
