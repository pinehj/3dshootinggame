using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 2. 발사 위치에 수류탄 생성
    // 3. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    [Header("설정값")]
    public PlayerFireDataSO PlayerFireDataSO;

    public GameObject FireBombPosition;
    public GameObject BombPrefab;

    private Animator _animator;

    [Header("무기")]
    [SerializeField] private Weapon _weapon;

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
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        BombCount = PlayerFireDataSO.MaxBombCount;
        UIManager.Instance.InitializeBombChargeSlider(PlayerFireDataSO.MaxBombChargeTime);
        BombChargeTime = 0;

        _weapon.Equip();
    }
    private void Update()
    {
        if (UnityEngine.Cursor.lockState == CursorLockMode.Locked)
        {
            FireBullet();
            FireBomb();
        }
    }
    private void FireBullet()
    {
        if (InputManager.Instance.GetMouseButton(0))
        {
            _weapon.Primary();
        }

        if (InputManager.Instance.GetMouseButtonUp(0))
        {
            _weapon.StopAttack();
        }
    }

    private void FireBomb()
    {
        if (InputManager.Instance.GetMouseButton(1) && BombCount > 0)
        {
            BombChargeTime += Time.deltaTime;
        }
        if (InputManager.Instance.GetMouseButtonUp(1) && BombCount > 0)
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


}
