using UnityEngine;

public class StartEquipableGiver : MonoBehaviour
{

    public Weapon[] weapons;
    public Item[] items;
    public Talisman[] charms;


    void Start()
    {
        foreach (Weapon weapon in weapons) InventoryManager.ownedWeapons.Add(weapon);
        foreach (Item item in items) InventoryManager.ownedItems.Add(item);
        foreach (Talisman charm in charms) InventoryManager.ownedTalismas.Add(charm);

    }
}
