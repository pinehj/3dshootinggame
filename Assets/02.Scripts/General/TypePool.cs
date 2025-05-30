using Unity.VisualScripting;
using UnityEngine;

public class TypePool<T> : Pool<T> where T:MonoBehaviour, IInitializable, IType
{
    public override T GetFromPool(Vector3 position, int type)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].isActiveAndEnabled && _pool[i].GetEnumType() == type)
            {
                T poolObject = _pool[i];
                poolObject.transform.position = position;
                poolObject.Initialize();
                poolObject.gameObject.SetActive(true);
                return poolObject;
            }
        }
        T newPoolObject = Instantiate(_poolPrefabs[type], transform);
        _pool.Add(newPoolObject);
        newPoolObject.transform.position = position;
        newPoolObject.Initialize();
        newPoolObject.gameObject.SetActive(true);
        return newPoolObject;
    }
}
