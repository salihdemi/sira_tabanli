using UnityEngine;

public class StartEquipableGiver : MonoBehaviour
{

    public Weapon[] weapons;
    public Item[] items;
    public Charm[] charms;


    void Start()
    {
        foreach (Weapon weapon in weapons) InventoryManager.weapons.Add(weapon);
        foreach (Item item in items) InventoryManager.items.Add(item);
        foreach (Charm charm in charms) InventoryManager.charms.Add(charm);

    }
}
