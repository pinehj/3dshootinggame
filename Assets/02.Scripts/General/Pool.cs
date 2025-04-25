using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : Singleton<Pool<T>> where T:MonoBehaviour
{
    [SerializeField] protected T[] _poolPrefabs;
    [SerializeField] protected int poolSize;
    protected int _totalPoolSize = 0;
    [SerializeField] protected List<T> _pool;

    protected override void Awake()
    {
        base.Awake();

        InitializePool();
    }

    public virtual void InitializePool()
    {
        _totalPoolSize = poolSize * _poolPrefabs.Length;
        _pool = new List<T>(_totalPoolSize);
        for (int type = 0; type < _poolPrefabs.Length; ++type)
        {
            for (int count = 0; count < poolSize; ++count)
            {
                T poolObject = Instantiate(_poolPrefabs[type], transform);
                poolObject.gameObject.SetActive(false);
                _pool.Add(poolObject);
            }
        }
    }

    public virtual T GetFromPool(Vector3 position)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].isActiveAndEnabled)
            {
                T poolObject = _pool[0];
                poolObject.transform.position = position;
                poolObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                poolObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                poolObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                poolObject.gameObject.SetActive(true);
                return poolObject;
            }
        }
        T newPoolObject = Instantiate(_poolPrefabs[0], transform);
        _pool.Add(newPoolObject);
        newPoolObject.transform.position = position;
        newPoolObject.gameObject.SetActive(true);
        return newPoolObject;
    }
    public virtual T GetFromPool(Vector3 position, int index)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].isActiveAndEnabled)
            {
                T poolObject = _pool[0];


                poolObject.gameObject.SetActive(true);
                return poolObject;
            }
        }
        T newPoolObject = Instantiate(_poolPrefabs[0], transform);
        _pool.Add(newPoolObject);
        newPoolObject.transform.position = position;
        newPoolObject.gameObject.SetActive(true);
        return newPoolObject;
    }

    public virtual void ReturnToPool(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolObject.transform.position = transform.position;
        poolObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
