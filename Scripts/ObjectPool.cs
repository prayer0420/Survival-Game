using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public Transform spawnPoint;
        public int poolSize;
    }

    public List<Pool> Pools;
    public Dictionary<string, List<GameObject>> PoolDictionary;


    void Start()
    {
        PoolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (var pool in Pools)
        {
            List<GameObject> objectpool = new List<GameObject>();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.spawnPoint);
                obj.SetActive(false);
                objectpool.Add(obj);
            }

            PoolDictionary.Add(pool.tag, objectpool);
        }
    }

    public GameObject Get(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            return null;
        }

        List<GameObject> objectPool = PoolDictionary[tag];
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }

    public void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}