using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Food> useableFoods = new List<Food>();

    public void AddFood(Food food)
    {
        food.piece++;
        if(food.piece == 1)
        {
            useableFoods.Add(food);
        }
    }
    public void DecreaseFood(Food food)
    {
        food.piece--;
        if(food.piece == 0)
        {
            useableFoods.Remove(food);
        }
    }









    public List<Toy> useableToys = new List<Toy>();


    public void AddToy(Toy toy)
    {
        toy.piece++;
        if (toy.piece == 1)
        {
            useableToys.Add(toy);
        }
    }
    public void DecreaseToy(Toy toy)
    {
        toy.piece--;
        if (toy.piece == 0)
        {
            useableToys.Remove(toy);
        }
    }

}
