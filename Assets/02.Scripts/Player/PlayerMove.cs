using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _characterController;

    [Header("상태창")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _stamina = 100f;
    public float Stamina
    {
        get
        {
            return _stamina;
        }
        set
        {
            _stamina = Mathf.Clamp(value, 0, PlayerDataSO.MaxStamina);
            Debug.Log(_stamina);
            UIManager.Instance.UpdateStaminaSlider(_stamina);
        }
    }

    [Header("설정값")]
    public PlayerDataSO PlayerDataSO;
    private bool _isRunning = false;




    [Header("점프")]
    [SerializeField] private int _jumpCount = 0;
    [SerializeField] private bool _isJumping = false;


    [Header("구르기")]
    [SerializeField] private bool _isDashing = false;

    [Header("기어오르기")]
    [SerializeField] private bool _isClimbing = false;

    //중력
    private const float GRAVITY = -9.8f;
    private float _yVelocity = 0;

    private Vector3 dir = Vector3.zero;
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        UIManager.Instance.InitializePlayerStaminaSlider(PlayerDataSO.MaxStamina);
        Stamina = PlayerDataSO.MaxStamina;
    }
    // Update is called once per frame
    void Update()
    {
        SetDirection();
        Run();
        GroundCheck();
        Jump();
        Dash();
        Climb();
        Gravity();
        Move();
        RegenStamina();
    }

    public void SetDirection()
    {
        if (!_isDashing)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            dir = new Vector3(h, 0, v);
            dir = dir.normalized;


            // 메인 카메라 기준 방향
            dir = Camera.main.transform.TransformDirection(dir);
            //dir.y = 0;
        }
    }

    public void Run()
    {
        if (Input.GetButton("Run"))
        {
            _moveSpeed = PlayerDataSO.WalkSpeed + PlayerDataSO.RunSpeed * (Stamina / PlayerDataSO.MaxStamina);
            _isRunning = true;
            if (_characterController.velocity.x != 0 || _characterController.velocity.z != 0)
            {
                Stamina -= PlayerDataSO.RunStaminaCost * Time.deltaTime;
            }
        }
        else
        {
            if (_moveSpeed > PlayerDataSO.WalkSpeed)
            {
                _moveSpeed = PlayerDataSO.WalkSpeed;
                _isRunning = false;
            }
        }
    }

    public void GroundCheck()
    {
        // GroundCheck
        if (_characterController.isGrounded)
        {
            _isJumping = false;
            _jumpCount = 0;
        }
    }

    public void Jump()
    {
        // 점프
        if (Input.GetButtonDown("Jump") && _jumpCount < PlayerDataSO.MaxJumpCount && !_isClimbing)
        {
            _yVelocity = PlayerDataSO.JumpPower;
            _isJumping = true;
            _jumpCount++;
        }
    }

    public void Dash()
    {
        if (Input.GetButtonDown("Dash") && Stamina >= PlayerDataSO.DashStaminaCost && !_isDashing)
        {
            Stamina -= PlayerDataSO.DashStaminaCost;
            StartCoroutine(nameof(DashCoroutine));
        }
    }
    IEnumerator DashCoroutine()
    {
        _isDashing = true;
        if (dir == Vector3.zero)
        {
            dir = Vector3.forward;
            dir = Camera.main.transform.TransformDirection(dir);
        }
        dir *= PlayerDataSO.DashPower;
        yield return new WaitForSeconds(PlayerDataSO.DashDuration);
        _isDashing = false;
    }

    public void Climb()
    {
        if (!_isDashing && (_characterController.collisionFlags & CollisionFlags.Sides) != 0 && Stamina > 0)
        {
            _isClimbing = true;
            Stamina -= PlayerDataSO.ClimbStaminaCost * Time.deltaTime;
        }
        else
        {
            if (_isClimbing)
            {
                _isClimbing = false;
            }
        }
    }
    public void Gravity()
    {

        //Gravity
        if (!_characterController.isGrounded && !_isClimbing)
        {
            _yVelocity += GRAVITY * Time.deltaTime;
        }
        else if (!_isJumping)
        {
            _yVelocity = 0;
        }
        dir.y = _yVelocity;



    }

    public void Move()
    {
        if (!_isClimbing)
        {
            _characterController.Move(dir * _moveSpeed * Time.deltaTime);
        }
        else
        {
            dir.y = 1;
            _characterController.Move(dir * PlayerDataSO.ClimbSpeed * Time.deltaTime);
        }
    }

    public void RegenStamina()
    {
        if(_isRunning || _isDashing || _isClimbing || !_characterController.isGrounded)
        {
            return;
        }
        if (Stamina < PlayerDataSO.MaxStamina)
        {
            Stamina += PlayerDataSO.StaminaRegen * Time.deltaTime;
        }
    }
}
