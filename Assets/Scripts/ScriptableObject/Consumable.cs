using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable Objects/Consumables")]
public abstract class Consumable : ScriptableObject, ICollectibleReward
{
    public string _name;
    public Skill skill;

    public void Give()
    {
        InventoryManager.AddConsumable(this);
    }
}
