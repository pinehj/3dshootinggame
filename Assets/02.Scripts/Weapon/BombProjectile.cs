using Unity.VisualScripting;
using UnityEngine;

public class BombProjectile : MonoBehaviour, IInitializable
{
    public GameObject ExplosionEffectPrefab;
    public bool IsThrowed;
    public void Initialize()
    {
        transform.localRotation = Quaternion.identity;
        IsThrowed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsThrowed)
        {
            return;
        }
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        BombPool.Instance.ReturnToPool(this);
    }
}
