using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{
    [Header("연사")]
    [SerializeField] private int _burstFireCount;
    [SerializeField] private float _minVerticalRecoil;
    [SerializeField] private float _maxVerticalRecoil;
    [SerializeField] private float _verticalRecoilSpeed;

    [Header("이펙트")]
    public LineRenderer FireEffect;
    [SerializeField] private GameObject BulletEffectPrefab;
    [SerializeField] private float _reloadTimer;
    public float ReloadTimer
    {
        get
        {
            return _reloadTimer;
        }
        set
        {
            if (!_isEquiped)
            {
                return;
            }
            _reloadTimer = value;
            UIManager.Instance.UpdateBulletReloadSlider(_reloadTimer);
        }
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        _reloadTimer -= Time.deltaTime;
    }
    
    public override bool Primary()
    {
        if (_isReloading)
        {
            CancelReload();
        }
        return base.Primary();
    }
    public override void Alternate()
    {
    }

    public override void PerformAttack()
    {
        Ray ray;
        if (CameraManager.Instance.CurrentMode == ECameraMode.QV)
        {
            ray = new Ray(FireBulletPosition.transform.position, transform.forward);
        }
        else
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        RaycastHit hitInfo = new RaycastHit();

        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit)
        {
            GameObject BulletEffect = Instantiate(BulletEffectPrefab);

            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal;

            IDamageable damagedEntity = hitInfo.collider.GetComponent<IDamageable>();
            if (damagedEntity != null)
            {
                Damage damage = new Damage();
                damage.Value = 10;
                damage.KnockbackPower = 10;
                damage.From = gameObject;
                damage.HitPoint = hitInfo.point;

                damage.Normal = hitInfo.normal;
                damagedEntity.TakeDamage(damage);
            }
        }

        StopCoroutine(nameof(DeactiveFireEffectCoroutine));
        StartFireEffect(hitInfo, isHit);
        StartCoroutine(nameof(DeactiveFireEffectCoroutine), .03f);


        _attackTimer = 1 / WeaponData.RateOfFirePerSec;
        _currentMagazine--;

        _burstFireCount++;

        if (_burstFireCount > 3)
        {
            Camera.main.transform.GetComponent<CameraManager>().Rotate(new Vector2(0, Random.Range(_minVerticalRecoil, _maxVerticalRecoil)) * Time.deltaTime * _verticalRecoilSpeed);
        }
    }


    public override void Reload()
    {
        if (InputManager.Instance.GetButtonDown("Reload") && _currentMagazine < WeaponData.MagazineCapacity && !_isReloading)
        {
            StartCoroutine(nameof(ReloadBulletCoroutine));
        }
    }

    private void CancelReload()
    {
        _isReloading = false;
        StopCoroutine(nameof(ReloadBulletCoroutine));
        _attackTimer = 0;
    }

    IEnumerator ReloadBulletCoroutine()
    {
        _isReloading = true;
        ReloadTimer = 0;
        while (ReloadTimer < WeaponData.ReloadTime)
        {
            ReloadTimer += Time.deltaTime;
            yield return null;
        }
        _currentMagazine = WeaponData.MagazineCapacity;
        _isReloading = false;
        ReloadTimer = 0;
        Debug.Log("Reload Complete");
    }

    private void StartFireEffect(RaycastHit hitInfo, bool isHit)
    {
        FireEffect.gameObject.SetActive(true);
        FireEffect.positionCount = 2;
        FireEffect.SetPosition(0, FireBulletPosition.transform.position);

        if (isHit)
        {
            FireEffect.SetPosition(1, hitInfo.point);
        }
        else
        {
            if (CameraManager.Instance.CurrentMode == ECameraMode.QV)
            {
                FireEffect.SetPosition(1, FireBulletPosition.transform.position + transform.forward * 1000f);
            }
            else
            {
                FireEffect.SetPosition(1, FireBulletPosition.transform.position + Camera.main.transform.forward * 1000f);
            }
        }

    }
    IEnumerator DeactiveFireEffectCoroutine(float lifeTime = .03f)
    {
        yield return new WaitForSeconds(lifeTime);
        FireEffect.gameObject.SetActive(false);
    }

    public override void StopAttack()
    {
        _burstFireCount = 0;
    }
}
