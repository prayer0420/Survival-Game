using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<ItemSlot> invenItems = new List<ItemSlot>();
    public List<ItemSlot> InvenItems { get { return invenItems; } }

    private int invenSize = 21;

    public UnityAction onInventoryChanged;
    public UnityAction onToggleSubUI;
    public bool IsEquipUiActive { get; private set; } = false;
    public bool IsCraftUiActive { get; private set; } = false;

    [Header("Inven UI")]
    public Transform slotParent;
    public GameObject slotUIPrefab;

    [SerializeField]
    public ItemDatabaseSO itemDatabase;

    public Dictionary<int, int> ownItemsCount = new Dictionary<int, int>();
    public Dictionary<int, int> OwnItemsCount { get { return ownItemsCount; } }

    private int curSelectSlot = -1;

    [Header("Inventory Info")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemIcon;
    public TextMeshProUGUI itemStatText;
    public GameObject useButton;
    public GameObject dropButton;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public Equipment equip;

    [HideInInspector]
    public CharacterBuffManager buffManager;

    public GameObject invenUi;

    public Storagy nowInteracteStoragy;

    public Transform dropPosition;

    private void Awake()
    {
        equip = GetComponent<Equipment>();
        buffManager = GetComponent<CharacterBuffManager>();

        for (int i = 0; i < itemDatabase.itemObjects.Length; i++)
        {
            ownItemsCount.Add(itemDatabase.itemObjects[i].itemId, 0);
        }
    }

    private void Start()
    {
        player = PlayerManager.Instance.Player;

        for(int i = 0; i < invenSize; i++)
        {
            invenItems.Add(new ItemSlot(this));
            invenItems[i].SetSlot(i);

            GameObject slotUI = Instantiate(slotUIPrefab, slotParent);
            slotUI.GetComponent<SlotUI>().SetupSlotUI(invenItems[i]);
        }

        ClearInfo();
        invenUi.SetActive(false);
    }

    //���� ���ɼ� : �ټ��� ������ �ѹ��� ȹ�� (������ 5�� �������� ����ϴ� ���� ��)
    public void AddItem(ItemData item)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.itemPickupClip);

        if (item.isStack && SearchStackSlot(item.itemId, out int index))
        {
            ownItemsCount[item.itemId] += 1;
            invenItems[index].AddItemCount();

            return;
        }

        if (SearchEmptySlot(out index))
        {
            ownItemsCount[item.itemId] += 1;
            invenItems[index].SetItem(item);
            return;
        }

        //TODO : �÷��̾���� ���� �ʿ�
        DropItem(item);

    }

    private bool SearchEmptySlot(out int index)
    {
        for(int i = 0; i < invenSize; i++)
        {
            if (invenItems[i].slotItem == null)
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }

    private bool SearchStackSlot(int itemId, out int index)
    {
        for(int i = 0 ; i < invenSize; i++)
        {
            if (invenItems[i].slotItem == null) continue;

            if (invenItems[i].slotItem.itemId == itemId && invenItems[i].slotItem.maxStackAmount > invenItems[i].itemCount)
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }

    //���� ����
    private void ClearInvenSlot(int index)
    {
        invenItems[index].ClearSlot();
    }

    //������ �����͸� ������� ����
    public void RemoveItem(ItemData item)
    {
        int index = SearchItemIndexInInven(item);

        if (index >= 0)
        {
            ownItemsCount[item.itemId] -= 1;
            invenItems[index].itemCount -= 1;

            if (invenItems[index].itemCount <= 0)
            {
                ClearInvenSlot(index);
                ClearInfo();
            }
        }
        else
        {
            Debug.Log("�߸��� ������ ���� �õ�");
        }
    }

    //���� �κ��丮���� �������� ������ ������ ����
    public void RemoveSelectSlotItem()
    {
        if (curSelectSlot < 0) return;

        invenItems[curSelectSlot].itemCount -= 1;
        if (invenItems[curSelectSlot].itemCount <= 0)
        {
            ClearInvenSlot(curSelectSlot);
            SelectItemSlot(-1);
            ClearInfo();
        }
    }

    //������ �������� �ִ� �ε��� ã��
    private int SearchItemIndexInInven(ItemData item)
    {
        for(int i = 0; i < invenSize; i++)
        {
            if (invenItems[i].slotItem == item)
            {
                return i;
            }
        }

        return -1;
    }

    //������ ������
    private void DropItem(ItemData item)
    {
        //TODO �÷��̾���� ���� - ������ ���� �ʿ�
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.itemPickupClip);

    }

    //������ ���ý�, ���� ������ ���� �����
    public void SelectItemSlot(int index)
    {
        if (index < 0)
        {
            if (curSelectSlot >= 100)
            {
                equip.SelectItem(-1);
            }
            else if(curSelectSlot >= 0)
            {
                invenItems[curSelectSlot].SelectSlot(false);
            }

            ClearInfo();
            curSelectSlot = index;
            return;
        }

        if (curSelectSlot >= 0)
        {
            if (curSelectSlot < 100)
            {
                invenItems[curSelectSlot].SelectSlot(false);
            }
            else
            {
                equip.SelectItem(-1);
            }
        }

        curSelectSlot = index;

        if (index >= 100)
        {
            equip.SelectItem(index - 100);
        }
        else
        {
            invenItems[index].SelectSlot(true);
            SetItemInfo(invenItems[index].slotItem);
        }
    }

    //������ ����â �ʱ�ȭ
    public void ClearInfo()
    {
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        itemStatText.text = string.Empty;
        itemIcon.enabled = false;
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //������ ����â ǥ��
    public void SetItemInfo(ItemData data)
    {
        itemNameText.text = data.itemName;
        itemDescriptionText.text = data.description;
        itemIcon.enabled = true;
        itemIcon.sprite = data.icon;

        itemStatText.text = data.infoStrategy.ItemInfoText();
        if (curSelectSlot < 100)
        {
            useButton.SetActive(data.useItemStrategy.ActiveButton());
            dropButton.SetActive(true);
        }
    }

    //������ ��� ��ư ���
    public void OnUseButtonClick()
    {
        invenItems[curSelectSlot].slotItem.useItemStrategy.UseItem(this);
        onInventoryChanged?.Invoke();
    }

    public void OnDropButtonClick()
    {
        DropItem(invenItems[curSelectSlot].slotItem);
        RemoveSelectSlotItem();
    }

    public void ToggleUI()
    {
        ClearInfo();
        SelectItemSlot(-1);
        invenUi.SetActive(!invenUi.activeInHierarchy);
        player.controller.ToggleCursor(invenUi.activeInHierarchy);
        //�κ��丮â�� ������ ����, ��� UI�� ��� ����
        if(invenUi.activeInHierarchy == false)
        {
            IsEquipUiActive = false;
            IsCraftUiActive = false;
            onToggleSubUI?.Invoke();
        }
    }

    //���â�� ������, ��������̸� �����ϱ�
    public void DeselectEquipSlopt()
    {
        if(curSelectSlot >= 100)
        {
            SelectItemSlot(-1);
        }
    }

    //���, ���� UI�� ���ļ� �� �� ����
    public void EnableEquipUI()
    {
        IsEquipUiActive = true;
        IsCraftUiActive = false;
        onToggleSubUI?.Invoke();
    }

    public void EnableCraftUI()
    {
        IsCraftUiActive = true;
        IsEquipUiActive = false;
        onToggleSubUI?.Invoke();
    }

    // ������ ���� ��������
    public int GetItemCount(ItemData item)
    {
        if (ownItemsCount.TryGetValue(item.itemId, out int count))
        {
            return count;
        }
        return 0;
    }

    public void ClearInventory()
    {
        foreach (ItemSlot slot in InvenItems)
        {
            slot.ClearSlot();
        }
        ownItemsCount.Clear();
        for (int i = 0; i < itemDatabase.itemObjects.Length; i++)
        {
            ownItemsCount.Add(itemDatabase.itemObjects[i].itemId, 0);
        }
        onInventoryChanged?.Invoke();
    }

}
