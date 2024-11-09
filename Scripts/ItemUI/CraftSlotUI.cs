using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlotUI : MonoBehaviour
{
    private CraftManager manager;
    public Image selectedImage;
    public Image icon;
    public int itemId;

    public bool IsSelected { get; set; } = false;

    public void SetupSlotUI(ItemData item, CraftManager _manager)
    {
        manager = _manager;
        selectedImage.enabled = false;
        icon.sprite = item.icon;
        itemId = item.itemId;
    }

    public void OnButtonClick()
    {
        manager.SelectSlot(itemId);
    }

    public void ControllSelectedImage()
    {
        selectedImage.enabled = IsSelected;
    }
}
