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

}