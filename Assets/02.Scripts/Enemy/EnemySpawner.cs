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
        yield return new WaitForSeconds(SpawnInterval);
        while (true)
        {
            //EnemyPool.Instance.GetFromPool(transform.position, Random.Range(0, ((int)EEnemyType.Count)));
            EnemyPool.Instance.GetFromPool(transform.position, (int)EEnemyType.Basic);

            yield return new WaitForSeconds(SpawnInterval);
        }
    }
}
