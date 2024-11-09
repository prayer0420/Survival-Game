using UnityEngine.UI;
using UnityEngine;



public class Temperature : Conditions
{
    private void Awake()
    {
        uiBar.fillAmount = GetCurFilled();
    }

    new float GetCurFilled()
    {
        return curFill / maxFill;
    }
}
