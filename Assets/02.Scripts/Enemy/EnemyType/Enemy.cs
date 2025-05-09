using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EEnemyType
{
    Basic,
    Follow,


    Count
}
public class Enemy : AStateMachineOwner, IDamageable, IInitializable, IType
{
    public EEnemyType Type;

    private GameObject _player;
    public GameObject Player => _player;
    private CharacterController _characterController;
    public CharacterController CharacterController => _characterController;
    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    private Vector3 _startPosition;
    public Vector3 StartPosition => _startPosition;
    public Transform[] PatrolPoints;
    public int PatrolPointsNum;
    public List<Vector3> PatrolPointPositions;
    public float WaitAndPatrolTime = 5f;

    public float FindDistance = 7f;
    public float ReturnDistance = 1f;
    public float AttackDistance = 2f;

    public float MoveSpeed = 3.3f;

    public float AttackCoolTime = 0.5f;
    public float DamageTime = 0.5f;
    public float DieTime = 2f;
    private Vector3 _damageOrigin;
    public Vector3 DamagedOrigin => _damageOrigin;
    private float _knockbackedPower;
    public float KnockbackedPower => _knockbackedPower;

    [SerializeField] private float _health;
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            HealthBar.fillAmount = _health / MaxHealth;
        }
    }
    public float MaxHealth;

    public Image HealthBar;


    private Animator _animator;
    public Animator Animator => _animator;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();

        InitializeStateMachine();
    }

    protected virtual void Start()
    {
        Type = EEnemyType.Basic;
    }

    private void Update()
    {
        _statemachine.Update();
    }


    public override void InitializeStates()
    {
        _stateDict = new Dictionary<EState, AState>
        {
            { EState.Idle, new EnemyIdleState(_statemachine, this) },
            { EState.Patrol, new EnemyPatrolState(_statemachine,this) },
            { EState.Trace, new EnemyTraceState(_statemachine, this) },
            { EState.Attack, new EnemyAttackState(_statemachine, this) },
            { EState.Return, new EnemyReturnState(_statemachine, this) },
            { EState.Damaged, new EnemyDamagedState(_statemachine, this) },
            { EState.Die, new EnemyDieState(_statemachine, this) },
        };
    }
    public override void InitializeStateMachine()
    {
   
        _statemachine = new EnemyStateMachine(this);
        InitializeStates();
        _statemachine.InitializeDict(_stateDict, EState.Idle);
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        _damageOrigin = damage.From.transform.position;
        _knockbackedPower = damage.KnockbackPower;
        if (_health <= 0)
        {
            _statemachine.ChangeState(EState.Die);
            Animator.SetTrigger("Die");

            return;
        }
        _statemachine.ChangeState(EState.Damaged);
        Animator.SetTrigger("Hit");

    }

    public int GetEnumType()
    {
        return (int)Type;
    }

    public void Initialize()
    {
        _health = MaxHealth;
        Agent.speed = MoveSpeed;
        List<Vector3> patrolPointPositions = new List<Vector3>();
        foreach (Transform patrolPoint in PatrolPoints)
        {
            patrolPointPositions.Add(patrolPoint.position);
        }

        PatrolPointPositions = patrolPointPositions;
        _startPosition = transform.position;
        transform.localRotation = Quaternion.identity;
    }
}
