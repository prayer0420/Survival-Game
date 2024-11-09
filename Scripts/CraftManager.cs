using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    public ItemDatabaseSO itemDatabase;

    private List<ItemData> craftItemData = new List<ItemData>();
    private Dictionary<int, GameObject> craftUIs = new Dictionary<int, GameObject>();
    private Dictionary<int, int> playerItems = new Dictionary<int, int>();

    private Inventory playerInven;

    public GameObject slotUIPrefab;
    public Transform craftUISlotParent;

    private CraftSlotUI curSelectedSlot;

    [Header("Craft UI")]
    public Transform infoUIParent;
    private CraftNeedItemInfoUI[] infoUis;

    public TextMeshProUGUI craftItemName;
    public GameObject craftButton;

    //���۰��� �����۵� ���
    private void Awake()
    {
        foreach(ItemData item in itemDatabase.itemObjects)
        {
            playerItems.Add(item.itemId, 0);

            if(item.isCraftable)
            {
                craftItemData.Add(item);

                GameObject slotUI = Instantiate(slotUIPrefab, craftUISlotParent);
                slotUI.GetComponent<CraftSlotUI>().SetupSlotUI(item, this);
                slotUI.SetActive(false);

                craftUIs.Add(item.itemId, slotUI);
            }
        }

        infoUis = new CraftNeedItemInfoUI[infoUIParent.childCount];

        for(int i = 0; i < infoUIParent.childCount; i++)
        {
            infoUis[i] = infoUIParent.GetChild(i).GetComponent<CraftNeedItemInfoUI>();
            infoUis[i].ClearInfo();
        }

        craftItemName.text = string.Empty;
        craftButton.SetActive(false);
    }

    private void Start()
    {
        playerInven = PlayerManager.Instance.Player.inventory;
        playerInven.onToggleSubUI += CloseUI;

        gameObject.SetActive(false);
    }

    //�κ��丮�� �������� ���Ҷ�, ���۰��� ��� ������Ʈ
    private void OnEnable()
    {
        if (playerInven != null)
        {
            playerInven.onInventoryChanged += UpdateCraftMenu;
        }
    }

    private void OnDisable()
    {
        if (playerInven != null)
        {
            playerInven.onInventoryChanged -= UpdateCraftMenu;
        }
    }

    public void UpdateCraftMenu()
    {
        //���� �޴��� �� ��, �÷��̾� �κ��丮�� ������ ������ Ȯ��
        for (int i = 0; i < playerInven.InvenItems.Count; i++)
        {
            ItemSlot itemSlot = playerInven.InvenItems[i];
            if (itemSlot.slotItem != null)
            {
                playerItems = playerInven.OwnItemsCount;
            }
        }

        for(int i = 0; i < craftItemData.Count; i++)
        {
            if(CheckCraftable(craftItemData[i]))
            {
                //TODO : UI ���� (���� ����)
                craftUIs[craftItemData[i].itemId].SetActive(true);
            }
            else
            {
                //TODO : UI ���� (���� �Ұ���)
                craftUIs[craftItemData[i].itemId].SetActive(false);
            }
        }
    }

    //�������� ���۰��� ���� Ȯ��
    private bool CheckCraftable(ItemData craftItem)
    {
        for(int i = 0; i < craftItem.craftDatas.Length; i++)
        {
            if (playerItems[craftItem.craftDatas[i].material.itemId] < craftItem.craftDatas[i].needCount)
                return false;
        }

        return true;
    }

    //����â ����
    public void SelectSlot(int itemId)
    {
        if(itemId < 0)
        {
            curSelectedSlot.IsSelected = false;
            curSelectedSlot.ControllSelectedImage();
            curSelectedSlot = null;
            SetCraftInfoUI();
            craftButton.SetActive(false);
            return;
        }

        if(curSelectedSlot != null)
        {
            curSelectedSlot.IsSelected = false;
            curSelectedSlot.ControllSelectedImage();
        }

        curSelectedSlot = craftUIs[itemId].GetComponent<CraftSlotUI>();
        curSelectedSlot.IsSelected = true;
        curSelectedSlot.ControllSelectedImage();
        craftButton.SetActive(true);

        SetCraftInfoUI();
    }

    //���� ���� ǥ��
    private void SetCraftInfoUI()
    {
        if (curSelectedSlot == null)
        {
            foreach(var infoUi in infoUis)
            {
                infoUi.ClearInfo();
            }

            craftItemName.text = string.Empty;
            return;
        }

        ItemData itemData = itemDatabase.GetItemdataById(curSelectedSlot.itemId);

        craftItemName.text = itemData.itemName;
        for (int i = 0; i < infoUis.Length; i++)
        {
            if (i < itemData.craftDatas.Length)
            {
                infoUis[i].SetInfo(itemData.craftDatas[i]);
            }
            else
            {
                infoUis[i].ClearInfo();
            }
        }
    }

    //���۹�ư�� ������, ��Ḧ �Ҹ��ϰ� ���� �������� �κ��丮�� �߰�
    public void OnCraftButtonClick()
    {
        ItemData itemData = itemDatabase.GetItemdataById(curSelectedSlot.itemId);

        for (int i = 0; i < itemData.craftDatas.Length; i++)
        {
            for(int j = 0; j < itemData.craftDatas[i].needCount; j++)
            {
                playerInven.RemoveItem(itemData.craftDatas[i].material);
            }
        }

        playerInven.AddItem(itemData);

        if(!CheckCraftable(itemData))
        {
            SelectSlot(-1);
        }
    }

    public void ToggleUI()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        if(gameObject.activeInHierarchy == true)
        {
            if(playerInven.invenUi.activeInHierarchy == false)
            {
                playerInven.ToggleUI();
            }

            UpdateCraftMenu();
            playerInven.EnableCraftUI();
        }
    }

    //���â�� ������ �ݱ�
    public void CloseUI()
    {
        if(playerInven.IsCraftUiActive == false)
        {
            gameObject.SetActive(false);
        }
    }
}
