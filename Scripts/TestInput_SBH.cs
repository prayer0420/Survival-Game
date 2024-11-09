using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestInput_SBH : MonoBehaviour
{
    public Inventory inven;

    public ItemData apple;
    public ItemData axe;
    public ItemData stone;

    public EquipmentUI equipUi;
    public CraftManager craftUi;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inven.AddItem(apple);
            Debug.Log("아이템 지급");
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            inven.AddItem(axe);
        }
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            inven.AddItem(stone);
        }
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            equipUi.ToggleUI();
        }

        if(Input.GetKeyDown (KeyCode.I))
        {
            inven.ToggleUI();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            craftUi.ToggleUI();
        }
    }
}
