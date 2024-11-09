using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.playerData = CollectPlayerData();
        saveData.dayNightData = CollectDayNightData();
        saveData.droppedItemsData = CollectDroppedItemsData();
        saveData.inventoryData = CollectInventoryData();
        saveData.equipmentData = CollectEquipmentData();
        saveData.buildingData = CollectBuildingData(); 

        // JSON 직렬화 및 파일 저장
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("게임 저장 완료");
    }

    private PlayerData CollectPlayerData()
    {
        PlayerData playerData = new PlayerData();
        Player player = PlayerManager.Instance.Player;

        playerData.position = player.transform.position;
        playerData.rotation = player.transform.rotation;

        playerData.conditions = new ConditionData
        {
            health = player.conditions.hp.curFill,
            hunger = player.conditions.hunger.curFill,
            water = player.conditions.water.curFill,
            stamina = player.conditions.stamina.curFill,
            temperature = player.conditions.GetCurFill()
        };

        return playerData;
    }

    private DayNightData CollectDayNightData()
    {
        DayNightData dayNightData = new DayNightData
        {
            currentTime = DayNightCycle.Instance.GetCurrentHour()
        };
        return dayNightData;
    }

    private List<DroppedItemData> CollectDroppedItemsData()
    {
        List<DroppedItemData> droppedItemsData = new List<DroppedItemData>();
        DropItem[] droppedItems = FindObjectsOfType<DropItem>();

        foreach (DropItem drop in droppedItems)
        {
            DroppedItemData itemData = new DroppedItemData
            {
                itemId = drop.data.itemId,
                position = drop.transform.position,
                rotation = drop.transform.rotation
            };
            droppedItemsData.Add(itemData);
        }

        return droppedItemsData;
    }

    private InventoryData CollectInventoryData()
    {
        InventoryData inventoryData = new InventoryData();
        Inventory inventory = PlayerManager.Instance.Player.inventory;

        inventoryData.items = new List<InventoryItemData>();
        foreach (ItemSlot slot in inventory.InvenItems)
        {
            InventoryItemData itemData = new InventoryItemData();
            if (slot.slotItem != null)
            {
                itemData.itemId = slot.slotItem.itemId;
                itemData.quantity = slot.itemCount;
                itemData.isEmpty = false;
            }
            else
            {
                itemData.isEmpty = true;
            }
            // 슬롯 순서를 유지하기 위해 빈 슬롯도 포함
            inventoryData.items.Add(itemData);
        }

        return inventoryData;
    }


    private EquipmentData CollectEquipmentData()
    {
        EquipmentData equipmentData = new EquipmentData();
        Equipment equipment = PlayerManager.Instance.Player.equipment;

        equipmentData.equippedItems = new List<InventoryItemData>();
        foreach (ItemSlot slot in equipment.NowEquipItems)
        {
            InventoryItemData itemData = new InventoryItemData();
            if (slot.slotItem != null)
            {
                itemData.itemId = slot.slotItem.itemId;
                itemData.quantity = 1; 
                itemData.isEmpty = false;
            }
            else
            {
                itemData.isEmpty = true;
            }
            equipmentData.equippedItems.Add(itemData);
        }

        return equipmentData;
    }


    private List<BuildingData> CollectBuildingData()
    {
        List<BuildingData> buildingDataList = new List<BuildingData>();
        Building[] buildings = FindObjectsOfType<Building>();

        foreach (Building building in buildings)
        {
            if (building.buildData != null && building.buildData.itemData != null)
            {
                BuildingData data = new BuildingData
                {
                    itemId = building.buildData.itemData.itemId,
                    position = building.transform.position,
                    rotation = building.transform.rotation
                };
                buildingDataList.Add(data);
            }
        }

        return buildingDataList;
    }
}
