using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _animator;
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
            _stamina = Mathf.Clamp(value, 0, PlayerMoveDataSO.MaxStamina);
            UIManager.Instance.UpdateStaminaSlider(_stamina);   
        }
    }

    [Header("설정값")]
    public PlayerMoveDataSO PlayerMoveDataSO;

    [Header("달리기")]
    [SerializeField] private bool _isRunning = false;

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
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        UIManager.Instance.InitializePlayerStaminaSlider(PlayerMoveDataSO.MaxStamina);
        Stamina = PlayerMoveDataSO.MaxStamina;
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
            float h =InputManager.Instance.GetAxis("Horizontal");
            float v = InputManager.Instance.GetAxis("Vertical");
            dir = new Vector3(h, 0, v);
            _animator.SetFloat("MoveH", h);
            _animator.SetFloat("MoveV", v);


            dir = dir.normalized;


            // 메인 카메라 기준 방향
            dir = Camera.main.transform.TransformDirection(dir);
            //dir.y = 0;
        }
    }

    public void Run()
    {
        if (InputManager.Instance.GetButton("Run"))
        {
            _moveSpeed = PlayerMoveDataSO.WalkSpeed + PlayerMoveDataSO.RunSpeed * (Stamina / PlayerMoveDataSO.MaxStamina);
            _isRunning = true;
            if (_characterController.velocity.x != 0 || _characterController.velocity.z != 0)
            {
                Stamina -= PlayerMoveDataSO.RunStaminaCost * Time.deltaTime;
            }
        }
        else
        {
            if (_isRunning)
            {
                _moveSpeed = PlayerMoveDataSO.WalkSpeed;
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
        if (InputManager.Instance.GetButtonDown("Jump") && _jumpCount < PlayerMoveDataSO.MaxJumpCount && !_isClimbing)
        {
            _yVelocity = PlayerMoveDataSO.JumpPower;
            _isJumping = true;
            _jumpCount++;
        }
    }

    public void Dash()
    {
        if (InputManager.Instance.GetButtonDown("Dash") && Stamina >= PlayerMoveDataSO.DashStaminaCost && !_isDashing)
        {
            Stamina -= PlayerMoveDataSO.DashStaminaCost;
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
        dir *= PlayerMoveDataSO.DashPower;
        yield return new WaitForSeconds(PlayerMoveDataSO.DashDuration);
        _isDashing = false;
    }

    public void Climb()
    {
        if (!_isDashing && (_characterController.collisionFlags & CollisionFlags.Sides) != 0 && Stamina > 0)
        {
            _isClimbing = true;
            Stamina -= PlayerMoveDataSO.ClimbStaminaCost * Time.deltaTime;
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
            _characterController.Move(dir * PlayerMoveDataSO.ClimbSpeed * Time.deltaTime);
        }
    }

    public void RegenStamina()
    {
        if(_isRunning || _isDashing || _isClimbing || !_characterController.isGrounded)
        {
            return;
        }
        if (Stamina < PlayerMoveDataSO.MaxStamina)
        {
            Stamina += PlayerMoveDataSO.StaminaRegen * Time.deltaTime;
        }
    }
}
