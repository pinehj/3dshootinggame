using Unity.VisualScripting;
using UnityEngine;

public class BombProjectile : MonoBehaviour, IInitializable
{
    public GameObject ExplosionEffectPrefab;

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        BombPool.Instance.ReturnToPool(this);
    }
}
