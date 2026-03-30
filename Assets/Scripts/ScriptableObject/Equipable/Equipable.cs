using UnityEngine;

public abstract class Equipable : ScriptableObject, ICollectibleReward
{
    public Sprite sprite;

    public virtual void Give()
    {
        if (this is Weapon) InventoryManager.ownedWeapons.Add((Weapon)this);
        else if (this is Item) InventoryManager.ownedItems.Add((Item)this);
        else if (this is Talisman) InventoryManager.ownedTalismas.Add((Talisman)this);
    }
}
