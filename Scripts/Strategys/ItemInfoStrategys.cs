public class EquipInfo : IItemInfoStrategy
{
    ItemData data;
    public EquipInfo(ItemData getData)
    {
        data = getData;
    }

    public string ItemInfoText()
    {
        string str = string.Empty;

        for (int i = 0; i < data.equipDatas.Length; i++)
        {
            switch (data.equipDatas[i].statusType)
            {
                case StatusType.Attack:
                    str += $"공격력 : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Defence:
                    str += $"방어력 : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Health:
                    str += $"최대체력 : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Stamina:
                    str += $"최대 스테미나 : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Speed:
                    str += $"이동속도 : {data.equipDatas[i].value} ";
                    break;
                case StatusType.TemResist_Low:
                    str += $"저온저항 : {data.equipDatas[i].value} ";
                    break;
                case StatusType.TemResist_High:
                    str += $"고온저항 : {data.equipDatas[i].value} ";
                    break;
            }

            if(i == 2)
            {
                str += "\n";
            }
        }

        return str;
    }
}

public class ConsumableInfo : IItemInfoStrategy
{
    ItemData data;
    public ConsumableInfo(ItemData getData)
    {
        data = getData;
    }
    public string ItemInfoText()
    {
        string str = string.Empty;
        for (int i = 0; i < data.consumableDatas.Length; i++)
        {
            switch (data.consumableDatas[i].consumType)
            {
                case ConsumableType.Health:
                    str += $"체력 회복 : {data.consumableDatas[i].value} ";
                    break;
                case ConsumableType.Hunger:
                    str += $"배고픔 회복 : {data.consumableDatas[i].value} ";
                    break;
                case ConsumableType.Water:
                    str += $"갈증 회복: {data.consumableDatas[i].value} ";
                    break;
                case ConsumableType.Buff:
                    str += $"버프지속시간 : {data.consumableDatas[i].value} ";
                    break;
            }
                    
        }

        return str;
    }
}

public class NoneInfo : IItemInfoStrategy
{
    public string ItemInfoText()
    {
        return string.Empty;
    }
}