using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InventoryManager
{






    public static Dictionary<Food, int> foods = new Dictionary<Food, int>();



    public static List<Weapon> weapons = new List<Weapon>();
    public static List<Item> items = new List<Item>();
    public static List<Charm> charms = new List<Charm>();




    public static void AddFood(Food food, int amount = 1)
    {
        if (foods.ContainsKey(food))//zaten varsa
        {
            foods[food] += amount;
        }
        else//yoksa ekle
        {
            foods.Add(food, amount);
        }
        Debug.Log($"{food.name} eklendi. Yeni adet: {foods[food]}");
    }
    public static void RemoveFood(Food food)
    {
        if (foods.ContainsKey(food))
        {
            foods[food] -= 1;
            if (foods[food] <= 0)
            {
                foods.Remove(food);
            }
        }
    }


    public static List<Food> GetOwnedFoods()
    {
        return foods.Where(pair => pair.Value > 0)
                .Select(pair => pair.Key)
                .ToList();
    }


    public static int GetFoodCount(Food food)
    {
        // Eðer toy null ise veya sözlükte yoksa 0 döndür, hata verme!
        if (food == null || !foods.ContainsKey(food)) return 0;
        return foods[food];
    }




    public static Dictionary<Toy, int> toys = new Dictionary<Toy, int>();


    public static void AddToy(Toy toy, int amount = 1)
    {
        if (toys.ContainsKey(toy))//zaten varsa
        {
            toys[toy] += amount;
        }
        else//yoksa ekle
        {
            toys.Add(toy, amount);
        }
        Debug.Log($"{toy.name} eklendi. Yeni adet: {toys[toy]}");
    }
    public static void RemoveToy(Toy toy)
    {
        if (toys.ContainsKey(toy))
        {
            toys[toy] -= 1;
            if (toys[toy] <= 0)
            {
                toys.Remove(toy);
            }
        }
    }


    public static List<Toy> GetOwnedToys()
    {
        return toys.Where(pair => pair.Value > 0)
                .Select(pair => pair.Key)
                .ToList();
    }


    public static int GetToyCount(Toy toy)
    {
        // Eðer toy null ise veya sözlükte yoksa 0 döndür, hata verme!
        if (toy == null || !toys.ContainsKey(toy)) return 0;
        return toys[toy];
    }


}