using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storagy : MonoBehaviour
{
    public ItemDatabaseSO ItemDatabase;
    private Inventory playerInven;

    private int storagyCapacity = 20;
    private int nowItemCount = 0;

    Dictionary<int, int> storagyItemAmount = new Dictionary<int, int>();
    

    private void Awake()
    {
        for(int i = 0; i < ItemDatabase.itemObjects.Length; i++)
        {
            storagyItemAmount.Add(ItemDatabase.itemObjects[i].itemId, 0);
        }    
    }

    //TODO: 인터렉션 연결
    public void OnInteracte()
    {
        playerInven = PlayerManager.Instance.Player.inventory;
        playerInven.nowInteracteStoragy = this;
    }

    public void CloseStoragy()
    {
        playerInven.nowInteracteStoragy = null;
        playerInven = null;
    }

    public bool PutInItem(ItemData item)
    {
        if(nowItemCount >= storagyCapacity)
        {
            return false;
        }

        nowItemCount++;
        storagyItemAmount[item.itemId]++;
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        storagyItemAmount[item.itemId]--;
        nowItemCount--;
        playerInven.AddItem(item);
    }
}
