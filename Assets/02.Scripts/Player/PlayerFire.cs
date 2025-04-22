using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 2. 발사 위치에 수류탄 생성
    // 3. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    [Header("설정값")]
    public PlayerFireDataSO PlayerFireDataSO;
    private Camera _mainCamera;

    public GameObject FireBombPosition;
    public GameObject BombPrefab;

    public GameObject FireBulletPosition;
    public GameObject BulletEffectPrefab;

    [Header("총알")]
    [SerializeField] private float _bulletFireTimer;
    [SerializeField] private int _bulletCount;
    public int BulletCount
    {
        get
        {
            return _bulletCount;
        }
        set
        {
            _bulletCount = Mathf.Clamp(value, 0, PlayerFireDataSO.MaxBulletCount);
            UIManager.Instance.UpdateBulletCount(_bulletCount, PlayerFireDataSO.MaxBulletCount);
        }
    }
    [SerializeField] private bool _isReloading;
    [SerializeField] private float _bulletReloadTimer;
    public float BulletReloadTimer
    {
        get
        {
            return _bulletReloadTimer;
        }
        set
        {
            _bulletReloadTimer = Mathf.Clamp(value, 0, PlayerFireDataSO.BulletReloadTime);
            UIManager.Instance.UpdateBulletReloadSlider(_bulletReloadTimer);
        }
    }
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
            _bombCount = Mathf.Clamp(value, 0, PlayerFireDataSO.MaxBombCount);
            UIManager.Instance.UpdateBombCount(_bombCount, PlayerFireDataSO.MaxBombCount);
        }
    }
    [SerializeField] private float _bombChargeTime;
    public float BombChargeTime
    {
        get
        {
            return _bombChargeTime;
        }
        set
        {
            _bombChargeTime = Mathf.Clamp(value, 0, PlayerFireDataSO.MaxBombChargeTime);
            UIManager.Instance.UpdateBombChargeSlider(_bombChargeTime);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        BulletCount = PlayerFireDataSO.MaxBulletCount;
        UIManager.Instance.InitializeBulletReloadSlider(PlayerFireDataSO.BulletReloadTime);
        BulletReloadTimer = 0;

        BombCount = PlayerFireDataSO.MaxBombCount;
        UIManager.Instance.InitializeBombChargeSlider(PlayerFireDataSO.MaxBombChargeTime);
        BombChargeTime = 0;
    }
    private void Update()
    {
        FireBullet();
        FireBomb();
        Reload();
    }
    private void FireBullet()
    {
        if(_bulletFireTimer > 0)
        {
            _bulletFireTimer -= Time.deltaTime;
            _bulletFireTimer = Mathf.Clamp(_bulletFireTimer, 0, PlayerFireDataSO.BulletFireInterval);
        }

        if (Input.GetMouseButton(0) && _bulletFireTimer == 0)
        {
            if (_isReloading)
            {
                StopCoroutine(nameof(ReloadBulletCoroutine));
                BulletReloadTimer = 0;
            }
            if (BulletCount > 0)
            {
                Ray ray = new Ray(FireBulletPosition.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo = new RaycastHit();

                bool isHit = Physics.Raycast(ray, out hitInfo);
                if (isHit)
                {
                    GameObject BulletEffect = Instantiate(BulletEffectPrefab);

                    BulletEffect.transform.position = hitInfo.point;
                    BulletEffect.transform.forward = hitInfo.normal;


                    // 게임 수학: 선형대수학(스칼라, 벡터
                }
                _bulletFireTimer = PlayerFireDataSO.BulletFireInterval;
                BulletCount--;

                //if (BulletCount == 0)
                //{
                //    StartCoroutine(nameof(ReloadBulletCoroutine));
                //}
            }
        }
    }

    private void Reload()
    {
        if(Input.GetButtonDown("Reload") && BulletCount < PlayerFireDataSO.MaxBulletCount)
        {
            StartCoroutine(nameof(ReloadBulletCoroutine));
        }
    }

    private void FireBomb()
    {
        if (Input.GetMouseButton(1) && BombCount > 0)
        {
            BombChargeTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(1) && BombCount > 0)
        {
            BombCount--;
            Bomb bomb = BombPool.Instance.GetFromPool(FireBombPosition.transform.position);

            Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
            bombRigidbody.AddForce((Camera.main.transform.forward + Camera.main.transform.up / 2)
                                    * (PlayerFireDataSO.ThrowBombPower + PlayerFireDataSO.MaxBombChargePower * (BombChargeTime/PlayerFireDataSO.MaxBombChargeTime))
                                    , ForceMode.Impulse);
            bombRigidbody.AddTorque(Vector3.one);

            BombChargeTime = 0;
        }
    }

    IEnumerator ReloadBulletCoroutine()
    {
        _isReloading = true;
        BulletReloadTimer = 0;
        while (BulletReloadTimer < PlayerFireDataSO.BulletReloadTime)
        {
            BulletReloadTimer += Time.deltaTime;
            yield return null;
        }
        BulletCount = PlayerFireDataSO.MaxBulletCount;
        _isReloading = false;
        BulletReloadTimer = 0;
        Debug.Log("Reload Complete");
    }
}
