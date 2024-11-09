using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureSpawnManager : Singleton<CreatureSpawnManager>
{
    public ObjectPool pool;
    public string[] animalTags;
    public float spawnRate;
    public float spawnRange;
    private float lastSpawnTime;

    protected override void Awake()
    {
        base.Awake();
        pool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > spawnRate)
        {
            lastSpawnTime = Time.time;
            SpawnAnimal();
        }
    }

    private void SpawnAnimal()
    {
        for (int i = 0; i < animalTags.Length; i++)
        {
            GameObject creature = pool.Get(animalTags[i]);
            if (creature == null) continue;
            else
            {
                //creature.transform.position = pool.Pools[i].spawnPoint.position;
                creature.transform.position = GetSpawnPosition(i);
                creature.transform.rotation = Quaternion.identity;
            }
        }
    }

    private Vector3 GetSpawnPosition(int index)
    {
        Vector3 randomPos = Random.insideUnitSphere * spawnRange;
        randomPos += pool.Pools[index].spawnPoint.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, spawnRange, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return pool.Pools[index].spawnPoint.position;
    }
}
