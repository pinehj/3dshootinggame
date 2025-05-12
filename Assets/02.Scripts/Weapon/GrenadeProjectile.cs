using Unity.VisualScripting;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour, IInitializable
{
    public GameObject ExplosionEffectPrefab;
    public bool IsThrowed;
    private Rigidbody _rigidbody;
    public void Initialize()
    {
        transform.localRotation = Quaternion.identity;
        IsThrowed = false;

        if(_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        _rigidbody.isKinematic = true;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsThrowed)
        {
            return;
        }
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        Debug.Log(collision.gameObject.name);
        GrenadePool.Instance.ReturnToPool(this);
    }
}
