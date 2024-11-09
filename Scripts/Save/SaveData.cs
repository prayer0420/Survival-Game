using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData playerData;
    public DayNightData dayNightData;
    public List<DroppedItemData> droppedItemsData;
    public InventoryData inventoryData;
    public EquipmentData equipmentData;
    public List<BuildingData> buildingData; 
}

[Serializable]
public class PlayerData
{
    public Vector3 position;
    public Quaternion rotation;
    public ConditionData conditions;
}

[Serializable]
public class ConditionData
{
    public float health;
    public float hunger;
    public float water;
    public float stamina;
    public float temperature;
}

[Serializable]
public class InventoryData
{
    public List<InventoryItemData> items;
}

[Serializable]
public class InventoryItemData
{
    public int itemId;
    public int quantity;
    public bool isEmpty; 
}


[Serializable]
public class EquipmentData
{
    public List<InventoryItemData> equippedItems;
}

[Serializable]
public class DayNightData
{
    public float currentTime;
}

[Serializable]
public class DroppedItemData
{
    public int itemId;
    public Vector3 position;
    public Quaternion rotation;
}

[Serializable]
public class BuildingData
{
    public int itemId;
    public Vector3 position;
    public Quaternion rotation;
}
