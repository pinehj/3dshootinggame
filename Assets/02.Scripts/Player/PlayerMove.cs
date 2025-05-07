using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private Animator _animator;

    public LayerMask groundLayer;
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
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        UIManager.Instance.InitializePlayerStaminaSlider(PlayerMoveDataSO.MaxStamina);
        Stamina = PlayerMoveDataSO.MaxStamina;
        _moveSpeed = PlayerMoveDataSO.WalkSpeed;
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
        Move();
        RegenStamina();
    }

    public void SetDirection()
    {
        if (!_isDashing)
        {
            float h =InputManager.Instance.GetAxisRaw("Horizontal");
            float v = InputManager.Instance.GetAxisRaw("Vertical");
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
            if (_rigidbody.linearVelocity.x != 0 || _rigidbody.linearVelocity.z != 0)
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
        if(Physics.Raycast(transform.position + _collider.center, Vector3.down, _collider.height/2 + 0.01f, groundLayer))
        {
            if (_rigidbody.linearVelocity.y >= 0)
            {
                return;
            }
            _isJumping = false;
            _jumpCount = 0;
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
        }
    }

    public void Jump()
    {
        // 점프
        if (InputManager.Instance.GetButtonDown("Jump") && _jumpCount < PlayerMoveDataSO.MaxJumpCount && !_isClimbing)
        {
            _rigidbody.AddForce(Vector3.up * PlayerMoveDataSO.JumpPower);
            
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
        if (!_isDashing && Physics.Raycast(transform.position, Vector3.forward, _collider.radius + 0.01f, groundLayer) && Stamina > 0 && dir != Vector3.zero)
        {
            _isClimbing = true;
            _rigidbody.useGravity = false;
            Stamina -= PlayerMoveDataSO.ClimbStaminaCost * Time.deltaTime;
        }
        else
        {
            if (_isClimbing)
            {
                _isClimbing = false;
                _rigidbody.useGravity = true;
            }
        }
    }

    public void Move()
    {
        if (!_isClimbing)
        {
            //_rigidbody.MovePosition(transform.position + new Vector3(dir.x, _rigidbody.linearVelocity.y, dir.z) * _moveSpeed * Time.deltaTime);
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0) + new Vector3(dir.x, 0, dir.z) * _moveSpeed;
            Debug.Log(_rigidbody.linearVelocity.y);
        }
        else
        {
            dir.y = 1;
            //_rigidbody.MovePosition(transform.position + dir * PlayerMoveDataSO.ClimbSpeed * Time.deltaTime);
            _rigidbody.linearVelocity = dir * PlayerMoveDataSO.ClimbSpeed;
        }
    }

    public void RegenStamina()
    {
        if(_isRunning || _isDashing || _isClimbing || !Physics.Raycast(transform.position + _collider.center, Vector3.down, _collider.height/2 + 0.01f, groundLayer))
        {
            return;
        }
        if (Stamina < PlayerMoveDataSO.MaxStamina)
        {
            Stamina += PlayerMoveDataSO.StaminaRegen * Time.deltaTime;
        }
    }
}
