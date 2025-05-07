using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // 2. 발사 위치에 수류탄 생성
    // 3. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    [Header("설정값")]
    public PlayerFireDataSO PlayerFireDataSO;



    private Animator _animator;

    [Header("무기")]
    [SerializeField] private Weapon _currentWeapon;

    [SerializeField] private Weapon _gun;
    [SerializeField] private Weapon _throw;
    [SerializeField] private Weapon _melee;

    private int _meleeLayerIndex;
    //[Header("폭탄")]
    //[SerializeField] private int _bombCount;
    //public int BombCount
    //{
    //    get
    //    {
    //        return _bombCount;
    //    }
    //    set
    //    {
    //        _bombCount = Mathf.Clamp(value, 0, PlayerFireDataSO.MaxBombCount);
    //        UIManager.Instance.UpdateBombCount(_bombCount, PlayerFireDataSO.MaxBombCount);
    //    }
    //}
    //[SerializeField] private float _bombChargeTime;
    //public float BombChargeTime
    //{
    //    get
    //    {
    //        return _bombChargeTime;
    //    }
    //    set
    //    {
    //        _bombChargeTime = Mathf.Clamp(value, 0, PlayerFireDataSO.MaxBombChargeTime);
    //        UIManager.Instance.UpdateBombChargeSlider(_bombChargeTime);
    //    }
    //}
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _meleeLayerIndex = _animator.GetLayerIndex("Melee Layer");
    }
    private void Start()
    {
        //BombCount = PlayerFireDataSO.MaxBombCount;
        //UIManager.Instance.InitializeBombChargeSlider(PlayerFireDataSO.MaxBombChargeTime);
        //BombChargeTime = 0;

        _currentWeapon.Equip();
    }
    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            UseWeapon();
            ChangeWeapon();
        }
    }
    private void UseWeapon()
    {
        if (InputManager.Instance.GetMouseButton(0))
        {
            if (_currentWeapon.Primary())
            {
                _animator.SetTrigger("Shot");
            }
        }

        if (InputManager.Instance.GetMouseButtonUp(0))
        {
            _currentWeapon.StopAttack();
        }

        if (InputManager.Instance.GetButtonDown("Reload"))
        {
            _currentWeapon.Reload();
        }
    }

    private void ChangeWeapon()
    {
        Weapon newWeapon = null;
        if (InputManager.Instance.GetKeyDown(KeyCode.Alpha1))
        {
            newWeapon = _gun;
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha2))
        {
            newWeapon = _throw;
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha3))
        {
            newWeapon = _melee;
            _animator.SetLayerWeight(_meleeLayerIndex, 1);
        }

        if (newWeapon != null && _currentWeapon != newWeapon)
        {
            _currentWeapon.Unequip();
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = newWeapon;
            _currentWeapon.Equip();
            _currentWeapon.gameObject.SetActive(true);
        }
    }
}
