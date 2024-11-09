using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 전체 데이터를 관리하고, 고유 ID를 부여하는 스크립터블 오브젝트
/// </summary>
[CreateAssetMenu(fileName ="ItemDatabase", menuName ="Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    public ItemData[] itemObjects;

    //데이터베이스에 아이템을 등록할 때 아이템의 아이디를 정해줌
    public void OnValidate()
    {
        for(int i = 0; i < itemObjects.Length; i++)
        {
            itemObjects[i].itemId = i;
            itemObjects[i].infoStrategy = SetInfoStrategy(itemObjects[i]);
            itemObjects[i].useItemStrategy = SetUseStrategy(itemObjects[i]);
        }
    }

    public ItemData GetItemdataById(int itemId)
    {
        return itemObjects[itemId];
    }

    private IItemInfoStrategy SetInfoStrategy(ItemData data)
    {
        IItemInfoStrategy infoStrategy;

        switch(data.itemType)
        {
            case ItemType.Equipable:
                infoStrategy = new EquipInfo(data);

                break;
            case ItemType.Consumable:
                infoStrategy = new ConsumableInfo(data);
                break;
            default:
                infoStrategy = new NoneInfo();
                break;
        }

        return infoStrategy;
    }

    private IUseItemStrategy SetUseStrategy(ItemData data)
    {
        IUseItemStrategy useStrategy;

        switch(data.itemType)
        {
            case ItemType.Equipable:
                useStrategy = new Equip(data);
                break;
            case ItemType.Consumable:
                useStrategy = new Consume(data);
                break;
            default:
                useStrategy = new NotUseable();
                break;
        }

        return useStrategy;
    }
}
