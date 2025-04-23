using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BombPool : Singleton<BombPool>
{
    public Bomb BombPrefab;
    [SerializeField] private int poolSize;
    [SerializeField] private List<Bomb> _bombPool;

    protected override void Awake()
    {
        base.Awake();

        _bombPool = new List<Bomb>(poolSize);
        for(int i = 0; i< poolSize; ++i)
        {
            Bomb bomb = Instantiate(BombPrefab, transform);
            bomb.gameObject.SetActive(false);
            _bombPool.Add(bomb);
        }
    }

    public Bomb GetFromPool(Vector3 position)
    {
        for (int i = 0; i < _bombPool.Count; i++)
        {
            if (!_bombPool[i].isActiveAndEnabled)
            {
                Bomb bomb = _bombPool[i];
                bomb.transform.position = position;
                bomb.gameObject.SetActive(true);
                return bomb;
            }
        }
        Bomb newBomb = Instantiate(BombPrefab, transform);
        _bombPool.Add(newBomb);
        newBomb.transform.position = position;
        newBomb.gameObject.SetActive(true);
        return newBomb;
    }

    public void ReturnToPool(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
        bomb.transform.position = transform.position;
    }
}
