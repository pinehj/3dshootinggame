using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : TypePool<Enemy>
{
    public override void InitializePool()
    {
        _totalPoolSize = poolSize * _poolPrefabs.Length;
        _pool = new List<Enemy>(_totalPoolSize);
        for (int type = 0; type < _poolPrefabs.Length; ++type)
        {
            for (int count = 0; count < poolSize; ++count)
            {
                Enemy poolObject = Instantiate(_poolPrefabs[type], transform);
                poolObject.Agent.enabled = false;
                poolObject.gameObject.SetActive(false);
                _pool.Add(poolObject);
            }
        }
    }
    public override void ReturnToPool(Enemy poolObject)
    {
        base.ReturnToPool(poolObject);
        poolObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        poolObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        poolObject.Agent.enabled = false;
    }

    public override Enemy GetFromPool(Vector3 position, int type)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].isActiveAndEnabled && _pool[i].GetEnumType() == type)
            {
                Enemy poolObject = _pool[i];
                poolObject.transform.position = position;
                poolObject.gameObject.SetActive(true);
                poolObject.Agent.enabled = true;

                return poolObject;
            }
        }
        Enemy newPoolObject = Instantiate(_poolPrefabs[type], transform);
        _pool.Add(newPoolObject);
        newPoolObject.transform.position = position;
        newPoolObject.gameObject.SetActive(true);
        newPoolObject.Agent.enabled = false;
        newPoolObject.Agent.enabled = true;

        return newPoolObject;
    }
}
