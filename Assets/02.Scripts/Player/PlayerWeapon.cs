using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // 2. 발사 위치에 수류탄 생성
    // 3. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    [Header("설정값")]
    public PlayerFireDataSO PlayerFireDataSO;

    private PlayerAnimationController _animationController;

    [Header("무기")]
    [SerializeField] private Weapon _currentWeapon;

    [SerializeField] private Weapon _gun;
    [SerializeField] private Weapon _throw;
    [SerializeField] private Weapon _melee;

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
        _animationController = GetComponentInChildren<PlayerAnimationController>();
    }
    private void Start()
    {
        //BombCount = PlayerFireDataSO.MaxBombCount;
        //UIManager.Instance.InitializeBombChargeSlider(PlayerFireDataSO.MaxBombChargeTime);
        //BombChargeTime = 0;

        _currentWeapon.Equip();

        _gun.Initialize();
        _melee.Initialize();
        _throw.Initialize();
        
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
                _animationController.SetTrigger("Shot");
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
            _animationController.SetLayerWeight("Melee Layer", 0);
            _animationController.SetLayerWeight("Shot Layer", 1);
            _animationController.SetLayerWeight("Grenade Layer", 0);


            _animationController.ShouldIK = true;
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha2))
        {
            newWeapon = _throw;
            _animationController.SetLayerWeight("Melee Layer", 0);
            _animationController.SetLayerWeight("Shot Layer", 0);
            _animationController.SetLayerWeight("Grenade Layer", 1);

            _animationController.ShouldIK = false;
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha3))
        {
            newWeapon = _melee;
            _animationController.SetLayerWeight("Melee Layer", 1);
            _animationController.SetLayerWeight("Shot Layer", 0);
            _animationController.SetLayerWeight("Grenade Layer", 0);

            _animationController.ShouldIK = false;
        }

        if (newWeapon != null && _currentWeapon != newWeapon)
        {
            _animationController.SetTrigger("AnimationReset");
            _currentWeapon.Unequip();
            _currentWeapon = newWeapon;
            _currentWeapon.Equip();
        }
    }

    public void PerformAttack()
    {
        _currentWeapon.PerformAttack();
    }

    public void Equip()
    {
        _currentWeapon.Equip();
    }
}
