using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DamageIndicator : MonoBehaviour
{
    public Image Image;
    public float flashSpeed;

    private Coroutine coroutine;

    private void Start()
    {
        PlayerManager.Instance.Player.conditions.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        Image.enabled = true;
        Image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            Image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        Image.enabled = false;
    }
}
