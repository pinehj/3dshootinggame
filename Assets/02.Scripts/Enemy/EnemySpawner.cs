using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float SpawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            EnemyPool.Instance.GetFromPool(transform.position, Random.Range(0, ((int)EEnemyType.Count)));
            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
