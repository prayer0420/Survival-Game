using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ��ü �����͸� �����ϰ�, ���� ID�� �ο��ϴ� ��ũ���ͺ� ������Ʈ
/// </summary>
[CreateAssetMenu(fileName ="ItemDatabase", menuName ="Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    public ItemData[] itemObjects;

    //�����ͺ��̽��� �������� ����� �� �������� ���̵� ������
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
