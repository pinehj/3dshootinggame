using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EEnemyType
{
    Basic,
    Follow,


    Count
}
public class Enemy : AStateMachineOwner, IDamageable, IType
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
    public float Health => _health;
    public float MaxHealth;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();

        InitializeStateMachine();
    }

    protected virtual void Start()
    {
        _health = MaxHealth;
        Agent.speed = MoveSpeed;
        Type = EEnemyType.Basic;

    }

    private void OnEnable()
    {
        List<Vector3> patrolPointPositions = new List<Vector3>();
        foreach (Transform patrolPoint in PatrolPoints)
        {
            patrolPointPositions.Add(patrolPoint.position);
        }

        PatrolPointPositions = patrolPointPositions;
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
        _health -= damage.Value;
        _damageOrigin = damage.From.transform.position;
        _knockbackedPower = damage.KnockbackPower;
        if (_health <= 0)
        {
            _statemachine.ChangeState(EState.Die);
            return;
        }
        _statemachine.ChangeState(EState.Damaged);
    }

    public int GetEnumType()
    {
        return (int)Type;
    }
}
