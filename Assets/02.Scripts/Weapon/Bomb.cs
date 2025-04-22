using UnityEngine;

public class Bomb : MonoBehaviour
{
    //목표: 마우스가 오른쪽 버튼을 누르면 카메라가 

    public GameObject ExplosionEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject =Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        BombPool.Instance.ReturnToPool(this);
    }
}
