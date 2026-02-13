using UnityEngine;

public class StartEquipableGiver : MonoBehaviour
{

    public Weapon[] weapons;
    public Item[] items;
    public Talisman[] talismans;


    void Start()
    {
        foreach (Weapon weapon in weapons) InventoryManager.ownedWeapons.Add(weapon);
        foreach (Item item in items) InventoryManager.ownedItems.Add(item);
        foreach (Talisman talisman in talismans) InventoryManager.ownedTalismas.Add(talisman);

    }
}
