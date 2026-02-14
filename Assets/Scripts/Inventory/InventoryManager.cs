using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InventoryManager
{






    public static Dictionary<Consumable, int> consumables = new Dictionary<Consumable, int>();



    public static List<Weapon> ownedWeapons = new List<Weapon>();
    public static HashSet<Weapon> equippedWeapons = new HashSet<Weapon>();

    public static List<Item> ownedItems = new List<Item>();
    public static HashSet<Item> equippedItems = new HashSet<Item>();

    public static List<Talisman> ownedTalismas = new List<Talisman>();
    public static HashSet<Talisman> equippedTalismans = new HashSet<Talisman>();


    public static void AddConsumable(Consumable consumable, int amount = 1)
    {

        if (consumables.ContainsKey(consumable))//zaten varsa
        {
            consumables[consumable] += amount;
        }
        else//yoksa ekle
        {
            consumables.Add(consumable, amount);
        }
        Debug.Log($"{consumable.name} eklendi. Yeni adet: {consumables[consumable]}");


        UpdateVisualizer();
    }


    public static void RemoveConsumable(Consumable consumable)
    {
        if (consumables.ContainsKey(consumable))
        {
            consumables[consumable] -= 1;
            if (consumables[consumable] <= 0)
            {
                consumables.Remove(consumable);
            }
        }
    }


    public static List<Consumable> GetOwnedConsumable()
    {
        return consumables.Where(pair => pair.Value > 0)
                .Select(pair => pair.Key)
                .ToList();
    }


    public static int GetConsumableCount(Consumable consumable)
    {
        // Eðer toy null ise veya sözlükte yoksa 0 döndür, hata verme!
        if (consumable == null || !consumables.ContainsKey(consumable)) return 0;
        return consumables[consumable];
    }




    public static bool IsWeaponEquipped(Weapon weapon)
    {
        return equippedWeapons.Contains(weapon);
    }
    public static bool IsItemEquipped(Item item)
    {
        return equippedItems.Contains(item);
    }
    public static bool IsTalismanEquipped(Talisman talisman)
    {
        return equippedTalismans.Contains(talisman);
    }


    public static void Equip(PersistanceStats character, Equipable equipable)
    {
        if (equipable is Weapon weapon)
        {
            if(character.weapon) equippedWeapons.Remove(character.weapon);
            character.weapon = weapon;
            equippedWeapons.Add(weapon);
        }
        else if (equipable is Item item)
        {
            if (character.item) equippedItems.Remove(character.item);
            character.item = item;
            equippedItems.Add(item);
        }
        else if (equipable is Talisman talisman)
        {
            if (character.talimsan) equippedTalismans.Remove(character.talimsan);
            character.talimsan = talisman;
            equippedTalismans.Add(talisman);
        }
    }


    private static InventoryVisualizer visualizer;

    private static void UpdateVisualizer()
    {
        if (visualizer == null)
        {
            GameObject go = new GameObject("InventoryVisualizer");
            visualizer = go.AddComponent<InventoryVisualizer>();

        }
        visualizer.consumables.Clear();
        visualizer.consumableNumbers.Clear();
        foreach (var pair in consumables)
        {
            visualizer.consumableNumbers.Add(pair.Key);
            visualizer.consumables.Add(pair.Value);
        }
        visualizer.ownedWeapons = ownedWeapons;
        visualizer.equippedWeapons = equippedWeapons;

        visualizer.ownedItems = ownedItems;
        visualizer.equippedItems = equippedItems;

        visualizer.ownedTalismas = ownedTalismas;
        visualizer.equippedTalismans = equippedTalismans;
    }

}
public class InventoryVisualizer : MonoBehaviour
{

    public List<Consumable> consumableNumbers = new List<Consumable>();
    public List<int> consumables = new List<int>();



    public List<Weapon> ownedWeapons = new List<Weapon>();
    public HashSet<Weapon> equippedWeapons = new HashSet<Weapon>();

    public List<Item> ownedItems = new List<Item>();
    public HashSet<Item> equippedItems = new HashSet<Item>();

    public List<Talisman> ownedTalismas = new List<Talisman>();
    public HashSet<Talisman> equippedTalismans = new HashSet<Talisman>();
}