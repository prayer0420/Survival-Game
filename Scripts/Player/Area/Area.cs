using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public enum AreaType
{
    Cold,
    Hot
}

public class Area : MonoBehaviour
{
    public float loaclTemperature;
    public float temperatureSpeed;
    public float damage;
    public float damageRate;

    private  float highTemperature = 60f;
    private  float lowTemperature = 10f;

    List<IDamagable> player = new List<IDamagable>();

    private void Start()
    {
        PlayerManager.Instance.Player.conditions.onOverHit += OverHitDamage;
        PlayerManager.Instance.Player.conditions.offOverHit += OverHitCancel;
    }

    public void OverHitDamage()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }
    public void OverHitCancel()
    {
        CancelInvoke();
    }

    public void DealDamage()
    {
        for(int i = 0; i < player.Count; i++)
        {
            player[i].TakeDamage(damage);
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent(out IDamagable damagable))
        {
            player.Add(damagable);
            PlayerManager.Instance.Player.conditions.area = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent(out IDamagable damagable))
        {
            player.Remove(damagable);       
            PlayerManager.Instance.Player.conditions.area = null;
        }
    }
}
