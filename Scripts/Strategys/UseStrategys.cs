using UnityEngine;

//장비 : 장착
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
            //TODO : 장착 시스템 제작
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

//소모품
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
                    Debug.Log($"체력 회복 {data.consumableDatas[i].value}");
                    inven.player.conditions.Heal(data.consumableDatas[i].value);
                    break;
                case ConsumableType.Hunger:
                    Debug.Log($"배고픔 회복 {data.consumableDatas[i].value}");
                    inven.player.conditions.Eat(data.consumableDatas[i].value);
                    break;
                case ConsumableType.Water:
                    Debug.Log($"갈증 회복 {data.consumableDatas[i].value}");
                    inven.player.conditions.Drink(data.consumableDatas[i].value);
                    break;
                case ConsumableType.Buff:
                    Debug.Log($"버프 사용 {data.consumableDatas[i].value}");
                    //TODO 캐릭터 버프 매니저 지정, 버프 관련 저장하기
                    inven.buffManager.AddBuffs(data.buffDatas);
                    break;
            }
        }

        inven.RemoveSelectSlotItem();
    }
}

//비소모성 사용 아이템 관련...일단 모양만 잡아두기
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

//사용불가능 아이템
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