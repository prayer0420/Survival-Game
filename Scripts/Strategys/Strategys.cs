public interface IItemInfoStrategy
{
    string ItemInfoText();
}

public interface IUseItemStrategy
{
    void UseItem(Inventory inven);

    bool ActiveButton();
}
