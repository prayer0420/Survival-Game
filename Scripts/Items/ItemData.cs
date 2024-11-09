using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]
    public int itemId;
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public GameObject dropPrefab;

    [Header("Stack")]
    public bool isStack;
    public int maxStackAmount;

    [Header("Craftable")]
    public bool isCraftable;
    public MaterialData[] craftDatas;

    [Header("Consumable")]
    public bool isConsumable;
    public ConsumableDate[] consumableDatas;
    public BuffData[] buffDatas;

    [Header("Equip")]
    public EquipType equipType;
    public GameObject equipPrefab;
    public EquipStatusData[] equipDatas;

    public IItemInfoStrategy infoStrategy;
    public IUseItemStrategy useItemStrategy;
}

[System.Serializable]
public class MaterialData
{
    public ItemData material;
    public int needCount;
}

[System.Serializable]
public class ConsumableDate
{
    public ConsumableType consumType;
    public float value;
}

[System.Serializable]
public class EquipStatusData
{
    public StatusType statusType;
    public float value;
}

[System.Serializable]
public class BuffData
{
    public int buffId;
    public BuffType buffType;
    public int value;
    public float time;
}
