using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    private Vector3 _startPosition;
    public Transform[] PatrolPoints;
    [SerializeField] private int _patrolIndex;
    public int PatrolPointsNum;
    private Coroutine _patrolCoroutine;

    public float WaitAndPatrolTime = 5f;

    public float FindDistance = 7f;
    public float ReturnDistance = 1f;
    public float AttackDistance = 2f;

    public float MoveSpeed = 3.3f;

    public float AttackCoolTime = 0.5f;
    private float _attackTimer = 0f;

    public float MaxHealtlh;
    private float _health;
    public float DamageTime = 0.5f;

    private Vector3 _damageOrigin;
    private float _knockbackedPower;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
    }

    private void Start()
    {
        _health = MaxHealtlh;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EEnemyState.Idle:
            {
                Idle();
                break;
            }
            case EEnemyState.Patrol:
            {
                Patrol();
                break;
            }
            case EEnemyState.Trace:
            {
                Trace();
                break;
            }
            case EEnemyState.Return:
            {
                Return();
                break;
            }
            case EEnemyState.Attack:
            {
                Attack();
                break;
            }
            case EEnemyState.Damaged:
            {
                Damaged();
                break;
            }
            case EEnemyState.Die:
            {
                break;
            }
        }
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.Value;
        _damageOrigin = damage.From.transform.position;
        _knockbackedPower = damage.KnockbackPower;
        if (_health <= 0)
        {
            CurrentState = EEnemyState.Die;
            Debug.Log($"상태전환: {CurrentState} -> Die");
            StartCoroutine(nameof(DieRoutine));

        }
        Debug.Log($"상태전환: {CurrentState} -> Damaged");

        //인터페이스로 구현할껄..
        CancelRoutine(_patrolCoroutine);
        _patrolCoroutine = null;
        //

        CurrentState = EEnemyState.Damaged;

        StopCoroutine(nameof(DamagedRoutine));
        StartCoroutine(nameof(DamagedRoutine));

    }
    private void Idle()
    {
        if (_patrolCoroutine == null)
        {
            _patrolIndex = 0;
            _patrolCoroutine = StartCoroutine(nameof(WaitPatrolRoutine));
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EEnemyState.Trace;


            //...
            //인터페이스로 구현할껄..
            CancelRoutine(_patrolCoroutine);
            _patrolCoroutine = null;
            //
            return;
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Patorl -> Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }
        if (Vector3.Distance(transform.position, PatrolPoints[_patrolIndex].position) <= ReturnDistance)
        {
            Debug.Log("도착");
            _patrolIndex = (_patrolIndex + 1) % PatrolPointsNum;
            return;
        }
        Vector3 dir = (PatrolPoints[_patrolIndex].position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
    private void Trace()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > FindDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EEnemyState.Return;
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EEnemyState.Attack;
            return;
        }

        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        if (Vector3.Distance(transform.position, _startPosition) <= ReturnDistance)
        {
            Debug.Log("상태전환: Return -> Idle");
            CurrentState = EEnemyState.Idle;
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) > AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EEnemyState.Trace;
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCoolTime)
        {
            Debug.Log("공격");
            _attackTimer = 0;
        }
    }

    private void Damaged()
    {
        Vector3 dir = (transform.position - _damageOrigin).normalized;
        _characterController.Move(dir * _knockbackedPower * Time.deltaTime);
    }
    private IEnumerator DamagedRoutine()
    {
        yield return new WaitForSeconds(DamageTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EEnemyState.Trace;
    }

    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private IEnumerator WaitPatrolRoutine()
    {
        yield return new WaitForSeconds(WaitAndPatrolTime);
        Debug.Log("상태전환: Idle -> Patrol");

        //...
        //인터페이스로 구현할껄..
        _patrolCoroutine = null;
        ///
        CurrentState = EEnemyState.Patrol;
    }

    private void CancelRoutine(Coroutine coroutine)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            Debug.Log("Cancel");
        }
    }

}
