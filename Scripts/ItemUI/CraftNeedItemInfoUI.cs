using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftNeedItemInfoUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameAndCount;

    public void SetInfo(MaterialData data)
    {
        icon.enabled = true;
        icon.sprite = data.material.icon;
        string str = $"{data.material.itemName} - X {data.needCount}";
        itemNameAndCount.text = str;
    }

    public void ClearInfo()
    {
        icon.enabled = false;
        itemNameAndCount.text = string.Empty;
    }
}
