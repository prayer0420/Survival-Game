using UnityEngine;
using UnityEngine.UI;


public class Conditions : MonoBehaviour
{
    public float curFill;
    public float maxFill;
    public float startFill;
    public float passiveFill;
    public Image uiBar;

    public float result;

    private float waterSpeed = 3;

    private void Start()
    {
        curFill = startFill;
    }
    private void Update()
    {
        uiBar.fillAmount = GetCurFilled();                                                
    }

    public void Add(float amount)
    {
        curFill = Mathf.Min(curFill + amount, maxFill);
    }

    public void Sub(float amount)
    {
        curFill = Mathf.Max(curFill - amount, 0f);
    }

    public void Set(float amount)
    {
        if (curFill > startFill)
        {
            Sub(amount);
        }
        else
        {
            Add(amount);
        }
    }

    public void WaterSet(float amount)
    {
        if(curFill > 0f)
        {
            PlayerManager.Instance.Player.controller.moveSpeed = waterSpeed;
        }
        else
        {
            PlayerManager.Instance.Player.controller.moveSpeed = 
                PlayerManager.Instance.Player.controller.moveSpeed;
        }
    }

    public float GetCurFilled()
    {
        return curFill / maxFill;
    }
}
