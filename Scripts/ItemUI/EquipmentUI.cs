using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public SlotUI[] equipSlotUIs = new SlotUI[4];

    private Equipment equip;
    private Inventory inven;
    private PlayerConditions condition;

    private CharacterBuffManager buffManager;

    [Header("Status")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI thirstyText;
    public TextMeshProUGUI atkUpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI temRisistText;

    private void Start()
    {
        inven = PlayerManager.Instance.Player.inventory;
        equip = inven.equip;
        buffManager = PlayerManager.Instance.Player.buffManager;
        condition = PlayerManager.Instance.Player.conditions;

        SetSlots();

        inven.onToggleSubUI += CloseUI;
        gameObject.SetActive(false);
    }

    //��� �����Ҷ� ���� ������ ������Ʈ
    private void OnEnable()
    {
        if (equip != null)
        {
            equip.onEquipChange += UpdateTexts;
            equip.onEquipChange -= ToggleUI;
        }

        if (buffManager != null)
        {
            buffManager.onStatusUpdate += UpdateTexts;
        }
    }

    //��� �����Ҷ�, ���â UI Ȱ��ȭ
    private void OnDisable()
    {
        if (equip != null)
        {
            equip.onEquipChange -= UpdateTexts;
            equip.onEquipChange += ToggleUI;
        }

        if (buffManager != null)
        {
            buffManager.onStatusUpdate -= UpdateTexts;
        }
    }

    //��� UI�� ��� ���� ��������
    private void SetSlots()
    {
        for (int i = 0; i < equipSlotUIs.Length; i++)
        {
            equipSlotUIs[i].SetupSlotUI(equip.NowEquipItems[i]);
        }
    }

    //�������� ��� ������ ������Ʈ
    public void UpdateEquipUis()
    {
        foreach(var ui in equipSlotUIs)
        {
            ui.UpdateSlotUI();
        }
    }

    //�������ͽ� ������Ʈ
    public void UpdateTexts()
    {
        healthText.text = $"ü�� : {condition.hp.curFill:0}/{condition.hp.maxFill:0}";
        staminaText.text = $"{condition.stamina.curFill:0}/{condition.stamina.maxFill:0}";
        hungerText.text = $"����� {condition.hunger.curFill:0}/{condition.hunger.maxFill:0}";
        thirstyText.text = $"���� {condition.water.curFill:0}/{condition.water.maxFill:0}";
        atkUpText.text = $"���ݷ� ���� + {buffManager.AttackUp:0}";
        defText.text = $"���� : {buffManager.DefUp:0}";
        speedText.text = $"�ӵ� ���� + {buffManager.SpeedUp:0}";
        temRisistText.text = $"{buffManager.LowTemResist:0} ~ {buffManager.HighTemResist:0}";
    }

    public void ToggleUI()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        if(gameObject.activeInHierarchy == true)
        {
            if(inven.invenUi.activeInHierarchy == false)
            {
                inven.ToggleUI();
            }

            UpdateEquipUis();
            UpdateTexts();
            inven.EnableEquipUI();
        }
        else
        {
            inven.DeselectEquipSlopt();
        }
    }

    //����â�� ������ �� UI �ݱ�
    public void CloseUI()
    {
        if (inven.IsEquipUiActive == false)
        {
            gameObject.SetActive(false);
        }
    }
}
