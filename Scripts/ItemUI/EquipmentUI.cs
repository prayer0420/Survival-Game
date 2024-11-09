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

    //장비를 장착할때 장착 아이템 업데이트
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

    //장비를 장착할때, 장비창 UI 활성화
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

    //장비 UI에 띄울 슬롯 가져오기
    private void SetSlots()
    {
        for (int i = 0; i < equipSlotUIs.Length; i++)
        {
            equipSlotUIs[i].SetupSlotUI(equip.NowEquipItems[i]);
        }
    }

    //장착중인 장비 아이콘 업데이트
    public void UpdateEquipUis()
    {
        foreach(var ui in equipSlotUIs)
        {
            ui.UpdateSlotUI();
        }
    }

    //스테이터스 업데이트
    public void UpdateTexts()
    {
        healthText.text = $"체력 : {condition.hp.curFill:0}/{condition.hp.maxFill:0}";
        staminaText.text = $"{condition.stamina.curFill:0}/{condition.stamina.maxFill:0}";
        hungerText.text = $"배고픔 {condition.hunger.curFill:0}/{condition.hunger.maxFill:0}";
        thirstyText.text = $"갈증 {condition.water.curFill:0}/{condition.water.maxFill:0}";
        atkUpText.text = $"공격력 증가 + {buffManager.AttackUp:0}";
        defText.text = $"방어력 : {buffManager.DefUp:0}";
        speedText.text = $"속도 증가 + {buffManager.SpeedUp:0}";
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

    //제작창이 열릴때 이 UI 닫기
    public void CloseUI()
    {
        if (inven.IsEquipUiActive == false)
        {
            gameObject.SetActive(false);
        }
    }
}
