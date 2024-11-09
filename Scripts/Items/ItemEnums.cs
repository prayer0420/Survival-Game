//아이템 타입
public enum ItemType
{
    Etc,
    Consumable,
    Equipable,
    Construct
}

//장착 타입
public enum EquipType
{
    None,
    Weapon,
    Head,
    Body,
    Feet
}

//소모품 효과 타입
public enum ConsumableType
{
    Health,
    Hunger,
    Water,
    Buff
}

//장착 효과 타입
public enum StatusType
{
    Attack,
    Defence,
    Health,
    Stamina,
    Speed,
    TemResist_Low,
    TemResist_High
}

//버프 타입
public enum BuffType
{
    StaminaUp,
    SpeedUp,
    AttackUp,
    DefenceUp
}

//도구가 무엇을 할 수 있는지
public enum ToolType
{
    None,
    Axe,
    Pickaxe
}