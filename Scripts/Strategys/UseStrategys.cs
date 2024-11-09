using UnityEngine;

//��� : ����
public class Equip : IUseItemStrategy
{
    ItemData data;

    public Equip(ItemData setData)
    {
        data = setData;
    }

    public bool ActiveButton()
    {
        return true;
    }

    public void UseItem(Inventory inven)
    {
        switch(data.equipType)
        {
            //TODO : ���� �ý��� ����
            case EquipType.Weapon:
                inven.equip.EquipWeapon(data);
                inven.SelectItemSlot(100);
                break;
            case EquipType.Head:
                inven.equip.EquipHead(data);
                inven.SelectItemSlot(101);
                break;
            case EquipType.Body:
                inven.equip.EquipBody(data);
                inven.SelectItemSlot(102);
                break;
            case EquipType.Feet:
                inven.equip.EquipFeet(data);
                inven.SelectItemSlot(103);
                break;
        }
    }
}

//�Ҹ�ǰ
public class Consume : IUseItemStrategy
{
    ItemData data;

    public Consume(ItemData setData)
    {
        data = setData;
    }

    public bool ActiveButton()
    {
        return true;
    }

    public void UseItem(Inventory inven)
    {
        for(int i = 0; i < data.consumableDatas.Length; i++)
        {
            switch(data.consumableDatas[i].consumType)
            {
                case ConsumableType.Health:
                    Debug.Log($"ü�� ȸ�� {data.consumableDatas[i].value}");
                    inven.player.conditions.Heal(data.consumableDatas[i].value);
                    break;
                case ConsumableType.Hunger:
                    Debug.Log($"����� ȸ�� {data.consumableDatas[i].value}");
                    inven.player.conditions.Eat(data.consumableDatas[i].value);
                    break;
                case ConsumableType.Water:
                    Debug.Log($"���� ȸ�� {data.consumableDatas[i].value}");
                    inven.player.conditions.Drink(data.consumableDatas[i].value);
                    break;
                case ConsumableType.Buff:
                    Debug.Log($"���� ��� {data.consumableDatas[i].value}");
                    //TODO ĳ���� ���� �Ŵ��� ����, ���� ���� �����ϱ�
                    inven.buffManager.AddBuffs(data.buffDatas);
                    break;
            }
        }

        inven.RemoveSelectSlotItem();
    }
}

//��Ҹ� ��� ������ ����...�ϴ� ��縸 ��Ƶα�
public class NonConsume : IUseItemStrategy
{
    ItemData data;

    public NonConsume(ItemData setData)
    {
        data = setData;
    }

    public bool ActiveButton()
    {
        return true;
    }

    public void UseItem(Inventory inven)
    {
        
    }
}

//���Ұ��� ������
public class NotUseable : IUseItemStrategy
{
    public bool ActiveButton()
    {
        return false;
    }

    public void UseItem(Inventory inven)
    {
        
    }
}