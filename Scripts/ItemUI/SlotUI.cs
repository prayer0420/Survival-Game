using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image selectedImage;
    public Image icon;
    public TextMeshProUGUI quantityText;

    public ItemSlot slot;

    private Button slotButton;

    private void Awake()
    {
        slotButton = GetComponent<Button>();
    }


    public void SetupSlotUI(ItemSlot slotInfo)
    {
        slot = slotInfo;
        slot.inven.onInventoryChanged += UpdateSlotUI;
        slot.onSelectChange += UpdateSlotUI;

        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        if(slot.slotItem != null)
        {
            icon.enabled = true;
            icon.sprite = slot.slotItem.icon;
            quantityText.text = slot.itemCount > 1 ? slot.itemCount.ToString() : string.Empty;
            selectedImage.enabled = slot.IsSelected;
            slotButton.enabled = true;
        }
        else
        {
            icon.enabled = false;
            quantityText.text = string.Empty;
            selectedImage.enabled = false;
            slotButton.enabled = false;
        }
    }

    public void OnClickItem()
    {
        slot.inven.SelectItemSlot(slot.slotIndex);
    }
}
