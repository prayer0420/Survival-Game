using UnityEngine;

public class UICondions :MonoBehaviour
{
    public Conditions hp;
    public Conditions hunger;
    public Conditions water;
    public Conditions stamina;
    public Temperature temperature;

    private void Start()
    {
        PlayerManager.Instance.Player.conditions.uiConditons = this;
    }
}
