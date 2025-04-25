using UnityEngine;
using UnityEngine.AI;

public class Enemy : AStateMachineOwner, IDamageable
{
    public enum EEnemyState
    {
        Idle,
        Patrol,
        Trace,
        Return,
        Attack,
        Damaged,
        Die
    }

    public EEnemyState CurrentState = EEnemyState.Idle;

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

    public float WaitAndPatrolTime = 5f;

    public float FindDistance = 7f;
    public float ReturnDistance = 1f;
    public float AttackDistance = 2f;

    public float MoveSpeed = 3.3f;

    public float AttackCoolTime = 0.5f;

    public float MaxHealtlh;
    private float _health;
    public float DamageTime = 0.5f;
    public float DieTime = 2f;
    private Vector3 _damageOrigin;
    public Vector3 DamagedOrigin => _damageOrigin;
    private float _knockbackedPower;
    public float KnockbackedPower => _knockbackedPower;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();

        InitializeStateMachine();
    }

    private void Start()
    {
        _health = MaxHealtlh;
        Agent.speed = MoveSpeed;
    }

    private void Update()
    {
        _statemachine.Update();
    }

    public override void InitializeStateMachine()
    {
        _statemachine = new EnemyStateMachine(this);
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        _damageOrigin = damage.From.transform.position;
        _knockbackedPower = damage.KnockbackPower;
        if (_health <= 0)
        {
            Debug.Log($"상태전환: {CurrentState} -> Die");
            _statemachine.ChangeState(EState.Die);

        }
        Debug.Log($"상태전환: {CurrentState} -> Damaged");
        _statemachine.ChangeState(EState.Damaged);
    }

}
