using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Equipment : MonoBehaviour
{
    private ItemSlot[] nowEquipItems = new ItemSlot[4];
    public ItemSlot[] NowEquipItems { get { return nowEquipItems; } }

    public Inventory inven;
    public bool isPlayer;

    private int curSelectedIndex = -1;

    public GameObject equipButton;
    public GameObject unequipButton;

    public UnityAction onEquipChange;

    public EquipObject curEquipWeapon;

    private void Awake()
    {
        if (isPlayer == true)
        {
            inven = GetComponent<Inventory>();
        }

        for (int i = 0; i < nowEquipItems.Length; i++)
        {
            nowEquipItems[i] = new ItemSlot(inven);
            nowEquipItems[i].slotIndex = 100 + i;
        }
    }

    //���â���� ������ ����
    public void SelectItem(int index)
    {
        if(index < 0)
        {
            nowEquipItems[curSelectedIndex].SelectSlot(false);
            curSelectedIndex = index;
            unequipButton.SetActive(false);
            return;
        }

        equipButton.SetActive(false);
        unequipButton.SetActive(true);

        if(curSelectedIndex >= 0)
        {
            nowEquipItems[curSelectedIndex].SelectSlot(false);
        }

        nowEquipItems[index].SelectSlot(true);
        nowEquipItems[index].SelectSlot(true);
        inven.SetItemInfo(nowEquipItems[index].slotItem);
        curSelectedIndex = index;
    }

    //���� ����
    public void EquipWeapon(ItemData item)
    {
        ItemData temp = null;
        if (nowEquipItems[0].slotItem != null)
        {
            Destroy(curEquipWeapon.gameObject);
            curEquipWeapon = null;
            temp = nowEquipItems[0].slotItem;
        }

        nowEquipItems[0].slotItem = item;
        inven.RemoveSelectSlotItem();

        if(temp != null)
        {
            inven.AddItem(temp);
        }

        //TODO : ���� ���� ��ġ ��û�ϱ�
        GameObject weapon = Instantiate(item.equipPrefab, PlayerManager.Instance.Player.equipPosition);
        curEquipWeapon = weapon.GetComponent<EquipObject>();
        //TODO : CharacterBuffManager ��ũ��Ʈ Player�� �ް�, �Ļ�� ���� �����ϱ�
        curEquipWeapon.OnEquiped(PlayerManager.Instance.Player.buffManager);

        inven.onInventoryChanged.Invoke();
        onEquipChange.Invoke();
    }

    public void EquipHead(ItemData item)
    {
        ItemData temp = null;
        if (nowEquipItems[1] != null)
        {
            temp = nowEquipItems[1].slotItem;
        }

        nowEquipItems[1].slotItem = item;
        inven.RemoveSelectSlotItem();

        if (temp != null)
        {
            inven.AddItem(temp);
        }

        inven.onInventoryChanged.Invoke();
        onEquipChange.Invoke();
    }

    public void EquipBody(ItemData item)
    {
        ItemData temp = null;
        if (nowEquipItems[2] != null)
        {
            temp = nowEquipItems[2].slotItem;
        }

        nowEquipItems[2].slotItem = item;
        inven.RemoveSelectSlotItem();

        if (temp != null)
        {
            inven.AddItem(temp);
        }

        inven.onInventoryChanged.Invoke();
        onEquipChange.Invoke();
    }

    public void EquipFeet(ItemData item)
    {
        ItemData temp = null;
        if (nowEquipItems[3] != null)
        {
            temp = nowEquipItems[3].slotItem;
        }

        nowEquipItems[3].slotItem = item;
        inven.RemoveSelectSlotItem();

        if (temp != null)
        {
            inven.AddItem(temp);
        }

        inven.onInventoryChanged.Invoke();
        onEquipChange.Invoke();
    }

    public void UnEquipItem()
    {
        if(curSelectedIndex == 0)
        {
            Destroy(curEquipWeapon.gameObject);
            curEquipWeapon = null;
        }

        ItemData temp = nowEquipItems[curSelectedIndex].slotItem;
        nowEquipItems[curSelectedIndex].ClearSlot();
        inven.AddItem(temp);
        inven.SelectItemSlot(-1);
        unequipButton.SetActive(false);
        onEquipChange.Invoke();
    }
}
