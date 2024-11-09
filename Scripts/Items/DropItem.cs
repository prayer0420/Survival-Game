using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInterable
{
    public ItemData data;

    //TODO : IInteractable »Æ¿Œ
    public string GetInteractPrompt()
    {
        string str = $"{data.itemName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        PlayerManager.Instance.Player.inventory.AddItem(data);
        Destroy(gameObject);
    }
}
