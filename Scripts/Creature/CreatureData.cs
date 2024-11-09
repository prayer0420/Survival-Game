using UnityEngine;

public enum CreatureType
{
    Aggressive,
    Passive,
    Nonattack
}

[CreateAssetMenu(fileName = "Creature", menuName = "Creature/New Creature")]
public class CreatureData : ScriptableObject
{
    [Header("Stats")]
    public int id;
    public string creatureName;
    public float health;
    public float passiveHealthRecovery;
    public float walkSpeed;
    public float runSpeed;
    public CreatureType creatureType;

    [Header("Sight")]
    public float detectDistance;
    public float fieldOfView;

    [Header("Wander")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    public float activityAreaRadius;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    public float attackDistance;

    [Header("DropItem")]
    public ItemData[] dropOnDeath;
}