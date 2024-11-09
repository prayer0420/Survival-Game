using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBuffManager : MonoBehaviour
{
    private Equipment equips;
    //���� �������� ������ �����ϴ� ��ųʸ�
    private Dictionary<int, Coroutine> nowBuffs = new Dictionary<int, Coroutine>();

    public UnityAction onStatusUpdate;

    private void Start()
    {
        equips = PlayerManager.Instance.Player.inventory.equip;
        equips.onEquipChange += EquipStatusCheck;
    }

    //���ݷ�
    private float equipAttack = 0;
    private float buffAttack = 0;
    public float AttackUp =>  equipAttack + buffAttack;

    //����
    private float equipDef = 0;
    private float buffDef = 0;
    public float DefUp => equipDef + buffDef;

    //�ִ�ü��
    private float equipHealth = 0;
    private float buffHealth = 0;
    public float HealthUp => equipHealth + buffHealth;

    //�ִ� ���׹̳�
    private float equipSta = 0;
    private float buffSta = 0;
    public float StaminaUp => equipSta + buffSta;

    //�ӵ� ����
    private float equipSpeed = 0;
    private float buffSpeed = 0;
    public float SpeedUp => equipSpeed + buffSpeed;

    //���� ����
    private float equipLowTem = 0;
    private float buffLowTem = 0;
    public float LowTemResist => equipLowTem + buffLowTem;

    //��� ����
    private float equipHighTem = 0;
    private float buffHighTem = 0;
    public float HighTemResist => equipHighTem + buffHighTem;

    public void EquipStatusCheck()
    {
        float attack = 0;
        float def = 0;
        float health = 0;
        float sta = 0;
        float speed = 0;
        float lowTem = 0;
        float highTem = 0;

        for(int i = 0; i < equips.NowEquipItems.Length; i++)
        {
            if (equips.NowEquipItems[i].slotItem == null)
            {
                continue;
            }

            ItemData item = equips.NowEquipItems[i].slotItem;
            for (int j = 0; j < item.equipDatas.Length; j++)
            {
                switch (item.equipDatas[j].statusType)
                {
                    case StatusType.Attack:
                        if (item.equipType != EquipType.Weapon)
                        {
                            attack += item.equipDatas[j].value;
                        }
                        break;
                    case StatusType.Defence:
                        def += item.equipDatas[j].value;
                        break;
                    case StatusType.Health:
                        health += item.equipDatas[j].value;
                        break;
                    case StatusType.Stamina:
                        sta += item.equipDatas[j].value;
                        break;
                    case StatusType.Speed:
                        speed += item.equipDatas[j].value;
                        break;
                    case StatusType.TemResist_Low:
                        lowTem += item.equipDatas[j].value;
                        break;
                    case StatusType.TemResist_High:
                        highTem += item.equipDatas[j].value;
                        break;
                }
            }
        }

        equipAttack = attack;
        equipDef = def;
        equipHealth = health;
        equipSta = sta;
        equipSpeed = speed;
        equipLowTem = lowTem;
        equipHighTem = highTem;
    }

    //���� �߰�
    public void AddBuffs(BuffData[] buffs)
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            //���� ID�� ������ ������ �ش� ���� ����
            if (nowBuffs.ContainsKey(buffs[i].buffId))
            {
                StopCoroutine(nowBuffs[buffs[i].buffId]);
                SetBuffs(buffs[i], false);
            }

            //���� �߰�
            SetBuffs(buffs[i], true);
            Coroutine buffCoroutine = StartCoroutine(EndBuff(buffs[i].time, buffs[i]));
            nowBuffs.Add(buffs[i].buffId, buffCoroutine);
        }

        onStatusUpdate?.Invoke();
    }

    //������ ������ �����ϴ� �޼���
    private void SetBuffs(BuffData buff, bool isAdd)
    {
        float value = isAdd ? buff.value : buff.value * -1;

        switch (buff.buffType)
        {
            case BuffType.StaminaUp:
                buffSta += value;
                break;
            case BuffType.SpeedUp:
                buffSpeed += value; 
                break;
            case BuffType.AttackUp:
                buffAttack += value;
                break;
            case BuffType.DefenceUp:
                buffDef += value;
                break;
        }
        
    }

    //���� ���ӽð� �ڷ�ƾ
    private IEnumerator EndBuff(float time, BuffData data)
    {
        yield return new WaitForSeconds(time);
        SetBuffs(data, false);
        nowBuffs.Remove(data.buffId);
        onStatusUpdate?.Invoke();
    }
}
