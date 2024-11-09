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
                    str += $"���ݷ� : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Defence:
                    str += $"���� : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Health:
                    str += $"�ִ�ü�� : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Stamina:
                    str += $"�ִ� ���׹̳� : {data.equipDatas[i].value} ";
                    break;
                case StatusType.Speed:
                    str += $"�̵��ӵ� : {data.equipDatas[i].value} ";
                    break;
                case StatusType.TemResist_Low:
                    str += $"�������� : {data.equipDatas[i].value} ";
                    break;
                case StatusType.TemResist_High:
                    str += $"������� : {data.equipDatas[i].value} ";
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
                    str += $"ü�� ȸ�� : {data.consumableDatas[i].value} ";
                    break;
                case ConsumableType.Hunger:
                    str += $"����� ȸ�� : {data.consumableDatas[i].value} ";
                    break;
                case ConsumableType.Water:
                    str += $"���� ȸ��: {data.consumableDatas[i].value} ";
                    break;
                case ConsumableType.Buff:
                    str += $"�������ӽð� : {data.consumableDatas[i].value} ";
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