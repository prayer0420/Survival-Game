using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerConditions conditions;
    public Inventory inventory;
    public DamageIndicator damageIndicator;
    public CharacterBuffManager buffManager;
    public Equipment equipment;
    public Transform equipPosition;

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        conditions = GetComponent<PlayerConditions>();  
        inventory = GetComponent<Inventory>();  
        damageIndicator = GetComponent<DamageIndicator>();
        equipment = GetComponent<Equipment>();
        buffManager = GetComponent<CharacterBuffManager>();
    }
}
