using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

interface IDamagable
{
    void TakeDamage(float damageAmount);
}


public class PlayerConditions : MonoBehaviour, IDamagable
{
    public UICondions uiConditons;
    public Area area;

    public Conditions hp { get { return uiConditons.hp; } }
    public Conditions hunger { get { return uiConditons.hunger; } }
    public Conditions stamina { get { return uiConditons.stamina; } }
    public Conditions water { get { return uiConditons.water; } }
    Temperature temperature { get {  return uiConditons.temperature; } }

    private float highTemperature = 60f;
    private float lowTemperature = 10f;

    public float noHungerHpDecay;
    public event Action onTakeDamage;
    public event Action onOverHit;
    public event Action offOverHit;

    private void Update()
    {
        hunger.Sub(hunger.passiveFill * Time.deltaTime);
        stamina.Add(stamina.passiveFill * Time.deltaTime);
        water.Sub(water.passiveFill * Time.deltaTime);
        hp.Add(stamina.passiveFill * Time.deltaTime);

        WaterToSpeed();

        if (area)
        {
            AreaCheck();
            TeamperatureDamage();
        }
        else
        {
            TeamperatureChange();
        }

        if (hunger.curFill < 0f)
        {
            hp.Sub(noHungerHpDecay * Time.deltaTime);
        }

        if (hp.curFill < 0f)
        {
            Die();
        }
        // 밤 시간 동안 HP 감소
        HandleNightTimeHpDecay();
    }

    bool IsOverHit = false;

    public void TeamperatureDamage()
    {
        if (temperature.curFill > highTemperature || temperature.curFill < lowTemperature)
        {
            if (!IsOverHit)
            {
                IsOverHit = true;
                if (onOverHit != null)
                    onOverHit();
            }
        }
    }
    public void TeamperatureChange()
    {
        temperature.Set(temperature.startFill * Time.deltaTime);
        if (IsOverHit)
        {
            IsOverHit = false;
            if (offOverHit != null)
                offOverHit();
        }
    }

    public void AreaCheck()
    {
        if(area.loaclTemperature > temperature.startFill)
        {
            temperature.Add(area.temperatureSpeed * Time.deltaTime);
        }
        else if(area.loaclTemperature < temperature.startFill)
        {
            temperature.Sub(area.temperatureSpeed * Time.deltaTime);
        }
    }

    public void WaterToSpeed()
    {
        water.WaterSet(water.startFill  * Time.deltaTime);  
    }

    private void Die()
    {
        // 플레이어가 죽었을 때 도는 로직
    }

    public float GetCurFill()
    {
        return temperature.curFill;
    }    

    public void Heal(float amount)
    {
        hp.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        water.Add(amount);
    }

    public void TakeDamage(float damageAmout)
    {
        float readDamage = damageAmout - PlayerManager.Instance.Player.buffManager.DefUp;
        readDamage = readDamage < 0 ? 1 : readDamage;
        hp.Sub(readDamage);
        onTakeDamage?.Invoke();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.playerHitClip);
    }

    private void HandleNightTimeHpDecay()
    {
        if (DayNightCycle.Instance == null)
            return;

        float currentHour = DayNightCycle.Instance.GetCurrentHour();

        // 현재 시간이 밤 시간인지 확인 (22:00 ~ 6:00)
        if (IsNightTime(currentHour))
        {
            // 1시간당 HP 5 감소, 2초당 1시간 -> 초당 2.5 HP 감소
            float hpDecreaseRate = 5f / 2f; // 2.5 HP per second
            hp.Sub(hpDecreaseRate * Time.deltaTime);
        }
    }

    private bool IsNightTime(float hour)
    {
        // 밤 시간: 22:00 ~ 6:00
        return (hour >= 22f && hour < 24f) || (hour >= 0f && hour < 6f);
    }
}
