using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("저장 파일이 존재하지 않습니다!");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        ApplyPlayerData(saveData.playerData);
        ApplyDayNightData(saveData.dayNightData);
        ApplyDroppedItemsData(saveData.droppedItemsData);
        ApplyEquipmentData(saveData.equipmentData);
        ApplyInventoryData(saveData.inventoryData);
        ApplyBuildingData(saveData.buildingData); 

        Debug.Log("게임 불러오기 완료");

        GameManager.Instance.equipUI.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ApplyPlayerData(PlayerData playerData)
    {
        Player player = PlayerManager.Instance.Player;

        player.transform.position = playerData.position;
        player.transform.rotation = playerData.rotation;

        player.conditions.hp.curFill = playerData.conditions.health;
        player.conditions.hunger.curFill = playerData.conditions.hunger;
        player.conditions.water.curFill = playerData.conditions.water;
        player.conditions.stamina.curFill = playerData.conditions.stamina;
        player.conditions.uiConditons.temperature.curFill = playerData.conditions.temperature;
    }

    private void ApplyDayNightData(DayNightData dayNightData)
    {
        DayNightCycle.Instance.SetCurrentTime(dayNightData.currentTime);
    }

    private void ApplyDroppedItemsData(List<DroppedItemData> droppedItemsData)
    {
        DropItem[] existingDrops = FindObjectsOfType<DropItem>();
        foreach (DropItem drop in existingDrops)
        {
            Destroy(drop.gameObject);
        }

        // 저장된 아이템 생성
        foreach (DroppedItemData itemData in droppedItemsData)
        {
            ItemData item = PlayerManager.Instance.Player.inventory.itemDatabase.GetItemdataById(itemData.itemId);
            GameObject dropPrefab = item.dropPrefab;
            Instantiate(dropPrefab, itemData.position, itemData.rotation);
        }
    }

    private void ApplyInventoryData(InventoryData inventoryData)
    {
        Inventory inventory = PlayerManager.Instance.Player.inventory;

        inventory.ClearInventory();

        inventory.ownItemsCount.Clear();
        for (int i = 0; i < inventory.itemDatabase.itemObjects.Length; i++)
        {
            inventory.ownItemsCount.Add(inventory.itemDatabase.itemObjects[i].itemId, 0);
        }

        for (int i = 0; i < inventoryData.items.Count; i++)
        {
            InventoryItemData itemData = inventoryData.items[i];

            if (!itemData.isEmpty)
            {
                ItemData item = inventory.itemDatabase.GetItemdataById(itemData.itemId);
                inventory.InvenItems[i].SetItem(item);
                inventory.InvenItems[i].itemCount = itemData.quantity;
                inventory.ownItemsCount[itemData.itemId] += itemData.quantity;
            }
            else
            {
                inventory.InvenItems[i].ClearSlot();
            }
        }

        inventory.onInventoryChanged?.Invoke();
        
        inventory.invenUi.SetActive(false);
    }


    private void ApplyEquipmentData(EquipmentData equipmentData)
    {
        Equipment equipment = PlayerManager.Instance.Player.equipment;
    
        foreach (ItemSlot slot in equipment.NowEquipItems)
        {
            if (slot.slotItem != null)
            {
                if (slot.slotIndex == 100 && equipment.curEquipWeapon != null)
                {
                    Destroy(equipment.curEquipWeapon.gameObject);
                    equipment.curEquipWeapon = null;
                }
                slot.ClearSlot();
            }
        }
    
        // 저장된 장비 데이터 적용
        for (int i = 0; i < equipmentData.equippedItems.Count; i++)
        {
            InventoryItemData itemData = equipmentData.equippedItems[i];
            if (!itemData.isEmpty)
            {
                ItemData item = equipment.inven.itemDatabase.GetItemdataById(itemData.itemId);
                equipment.NowEquipItems[i].SetItem(item);
    
                // 무기인 경우 장착된 오브젝트 생성 (필요 시)
                if (i == 0 && item.equipPrefab != null)
                {
                    GameObject weapon = Instantiate(item.equipPrefab, PlayerManager.Instance.Player.equipPosition);
                    equipment.curEquipWeapon = weapon.GetComponent<EquipObject>();
                    equipment.curEquipWeapon.OnEquiped(PlayerManager.Instance.Player.buffManager);
                }
            }
        }
    
        equipment.onEquipChange?.Invoke();
    
     }


    private void ApplyBuildingData(List<BuildingData> buildingDataList)
    {
        Building[] existingBuildings = FindObjectsOfType<Building>();
        foreach (Building building in existingBuildings)
        {
            Destroy(building.gameObject);
        }

        BuildingManager buildingManager = FindObjectOfType<BuildingManager>();
        if (buildingManager == null)
        {
            Debug.LogError("BuildingManager not found in the scene.");
            return;
        }

        foreach (BuildingData data in buildingDataList)
        {
            BuildData buildData = buildingManager.GetBuildDataByItemId(data.itemId);
            if (buildData != null)
            {
                GameObject buildingPrefab = buildData.prefab;
                if (buildingPrefab != null)
                {
                    GameObject newBuilding = Instantiate(buildingPrefab, data.position, data.rotation);
                    Building buildingComponent = newBuilding.GetComponent<Building>();
                    if (buildingComponent != null)
                    {
                        buildingComponent.buildData = buildData;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No BuildData found for itemId {data.itemId}");
            }
        }
    }
}
