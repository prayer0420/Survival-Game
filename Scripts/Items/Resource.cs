using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public GameObject dropItemPrefab;

    [SerializeField]
    private int quantityPerHit;
    [SerializeField]
    private int maxCapacity;
    private int capacity;

    [SerializeField]
    private float regenerationRate;
    private float lastRegenTime;

    public ToolType toolToGather;

    [Header("Random Drop")]
    public GameObject randomDropItemPrefab;
    [SerializeField]
    private float dropRate;

    private void Awake()
    {
        capacity = maxCapacity;
    }

    private void Update()
    {
        if(capacity < maxCapacity && Time.time - lastRegenTime > regenerationRate)
        {
            RegenerateResource();
        }
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal, ToolType type)
    {
        //타입이 맞지 않는 도구는 자원을 주지 않음
        if (type != toolToGather) return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.gatherClip);

        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;
            if (capacity == maxCapacity) lastRegenTime = Time.time;

            capacity -= 1;

            Instantiate(dropItemPrefab, hitPoint + Vector3.up * 0.2f, Quaternion.LookRotation(hitNormal, Vector3.up));
            
            if (randomDropItemPrefab != null && Random.Range(0, 101f) < dropRate)
            {
                Instantiate(randomDropItemPrefab, hitPoint + Vector3.up * 0.3f, Quaternion.LookRotation(hitNormal, Vector3.up));
            }
        }
    }

    //지정한 시간마다 자원 재생
    private void RegenerateResource()
    {
        capacity++;
        lastRegenTime = Time.time;
    }

}
