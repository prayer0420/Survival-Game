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

    //수정 가능성 : 다수의 아이템 한번에 획득 (나무가 5개 묶음으로 드랍하는 형태 등)
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

        //TODO : 플레이어와의 연결 필요
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

    //슬롯 비우기
    private void ClearInvenSlot(int index)
    {
        invenItems[index].ClearSlot();
    }

    //아이템 데이터를 기반으로 삭제
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
            Debug.Log("잘못된 아이템 제거 시도");
        }
    }

    //현재 인벤토리에서 선택중인 슬롯의 아이템 삭제
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

    //지정한 아이템이 있는 인덱스 찾기
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

    //아이템 버리기
    private void DropItem(ItemData item)
    {
        //TODO 플레이어와의 연결 - 버리는 지점 필요
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.itemPickupClip);

    }

    //아이템 선택시, 이전 선택한 내용 지우기
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

    //아이템 정보창 초기화
    public void ClearInfo()
    {
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        itemStatText.text = string.Empty;
        itemIcon.enabled = false;
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //아이템 정보창 표기
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

    //아이템 사용 버튼 기능
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
        //인벤토리창을 닫을때 제작, 장비 UI를 모두 닫음
        if(invenUi.activeInHierarchy == false)
        {
            IsEquipUiActive = false;
            IsCraftUiActive = false;
            onToggleSubUI?.Invoke();
        }
    }

    //장비창을 닫을때, 장비선택중이면 해제하기
    public void DeselectEquipSlopt()
    {
        if(curSelectSlot >= 100)
        {
            SelectItemSlot(-1);
        }
    }

    //장비, 제작 UI는 곂쳐서 켤 수 없음
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

    // 아이템 수량 가져오기
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
