using System.Collections;
using UnityEngine;

public class Drum : MonoBehaviour, IDamageable
{
    public float MaxHealth;
    [SerializeField] private float _health;
    public GameObject ExplodeEffect;
    public float ExplodeRadius;
    private Rigidbody _rigidbody;
    public float ExplodePower;
    public float DeactiveTime;

    public LayerMask AttackTargetLayer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _health = MaxHealth;
    }

    public void TakeDamage(Damage damage)
    {
        if(_health <= 0)
        {
            return;
        }
        _health -= damage.Value;
        //_rigidbody.AddForceAtPosition(-damage.Normal * damage.KnockbackPower, damage.HitPoint, ForceMode.Impulse);
        _rigidbody.AddForce((transform.position - damage.From.transform.position).normalized * damage.KnockbackPower, ForceMode.Impulse);
        if(_health <= 0)
        {
            StartCoroutine(ExplodeRoutine());
        }
    }

    private IEnumerator ExplodeRoutine()
    {

        yield return new WaitForSeconds(.1f);
        
        GameObject explodeEffect = Instantiate(ExplodeEffect);
        explodeEffect.transform.position = transform.position;


        Collider[] hits = Physics.OverlapSphere(transform.position, ExplodeRadius, AttackTargetLayer);
        foreach(Collider hit in hits)
        {
            if(hit.transform.gameObject == gameObject)
            {
                continue;
            }
            IDamageable damagedEntity = hit.GetComponent<IDamageable>();
            if (damagedEntity != null)
            {
                Damage damage = new Damage();
                damage.Value = 50;
                damage.KnockbackPower = 50;
                damage.From = gameObject;

                damagedEntity.TakeDamage(damage);
            }
        }
        _rigidbody.AddForce(new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1)) * ExplodePower, ForceMode.Impulse);

        yield return new WaitForSeconds(DeactiveTime);
        gameObject.SetActive(false);
        Debug.Log(gameObject.name);

    }

}
