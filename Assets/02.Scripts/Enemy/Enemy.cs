using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Trace,
        Return,
        Attack,
        Damaged,
        Die
    }

    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;
    private CharacterController _characterController;

    private Vector3 _startPosition;
    public Transform[] PatrolPoints;
    [SerializeField] private int _patrolIndex;
    public int PatrolPointsNum;
    private Coroutine _patrolCoroutine;

    public float IdleTime = 5f;

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
            case EnemyState.Idle:
            {
                Idle();
                break;
            }
            case EnemyState.Patrol:
            {
                Patrol();
                break;
            }
            case EnemyState.Trace:
            {
                Trace();
                break;
            }
            case EnemyState.Return:
            {
                Return();
                break;
            }
            case EnemyState.Attack:
            {
                Attack();
                break;
            }
            case EnemyState.Damaged:
            {
                Damaged();
                break;
            }
            case EnemyState.Die:
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
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환: {CurrentState} -> Die");
            StartCoroutine(nameof(DieCoroutine));

        }
        Debug.Log($"상태전환: {CurrentState} -> Damaged");

        //인터페이스로 구현할껄..
        CancelCoroutine(_patrolCoroutine);
        _patrolCoroutine = null;
        //

        CurrentState = EnemyState.Damaged;

        StopCoroutine(nameof(DamagedCoroutine));
        StartCoroutine(nameof(DamagedCoroutine));

    }
    private void Idle()
    {
        if (_patrolCoroutine == null)
        {
            _patrolIndex = 0;
            _patrolCoroutine = StartCoroutine(nameof(WaitPatrolCoroutine));
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;


            //...
            //인터페이스로 구현할껄..
            CancelCoroutine(_patrolCoroutine);
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
            CurrentState = EnemyState.Trace;
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
            CurrentState = EnemyState.Return;
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EnemyState.Attack;
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
            CurrentState = EnemyState.Idle;
            return;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
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
            CurrentState = EnemyState.Trace;
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
    private IEnumerator DamagedCoroutine()
    {
        yield return new WaitForSeconds(DamageTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private IEnumerator WaitPatrolCoroutine()
    {
        yield return new WaitForSeconds(IdleTime);
        Debug.Log("상태전환: Idle -> Patrol");

        //...
        //인터페이스로 구현할껄..
        _patrolCoroutine = null;
        ///
        CurrentState = EnemyState.Patrol;
    }

    private void CancelCoroutine(Coroutine coroutine)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            Debug.Log("Cancel");
        }
    }

}
