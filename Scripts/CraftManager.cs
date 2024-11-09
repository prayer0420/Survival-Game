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

    //제작가능 아이템들 등록
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

    //인벤토리의 아이템이 변할때, 제작가능 목록 업데이트
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
        //제작 메뉴를 열 때, 플레이어 인벤토리의 아이템 정보를 확인
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
                //TODO : UI 조작 (제작 가능)
                craftUIs[craftItemData[i].itemId].SetActive(true);
            }
            else
            {
                //TODO : UI 조작 (제작 불가능)
                craftUIs[craftItemData[i].itemId].SetActive(false);
            }
        }
    }

    //아이템의 제작가능 여부 확인
    private bool CheckCraftable(ItemData craftItem)
    {
        for(int i = 0; i < craftItem.craftDatas.Length; i++)
        {
            if (playerItems[craftItem.craftDatas[i].material.itemId] < craftItem.craftDatas[i].needCount)
                return false;
        }

        return true;
    }

    //제작창 선택
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

    //제작 정보 표기
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

    //제작버튼을 누르면, 재료를 소모하고 제작 아이템을 인벤토리에 추가
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

    //장비창이 열릴때 닫기
    public void CloseUI()
    {
        if(playerInven.IsCraftUiActive == false)
        {
            gameObject.SetActive(false);
        }
    }
}
