using UnityEditor.SceneManagement;
using UnityEngine;

public class ColliderThing : MonoBehaviour
{
    public enum Type
    {
        characterUnlocker,
        foodGiver,
        toyGiver,
        equipableGiver
    }
    public Type type;

    public CharacterData characterToUnlock;
    public Food foodToGive;
    public Toy toyToGive;
    public Equipable equipable;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(type == Type.characterUnlocker)
        {
            if (characterToUnlock == null) return;
            PartyManager.UnlockAlly(characterToUnlock);
            Destroy(this);
        }
        else if (type == Type.foodGiver)
        {
            InventoryManager.AddFood(foodToGive);
        }
        else if (type == Type.toyGiver)
        {
            InventoryManager.AddToy(toyToGive);
        }
        else if (type == Type.equipableGiver)
        {
            if (equipable is Weapon)
            {
                InventoryManager.weapons.Add((Weapon)equipable);
            }
            else if (equipable is Item)
            {
                InventoryManager.items.Add((Item)equipable);
            }
            else if (equipable is Charm)
            {
                InventoryManager.charms.Add((Charm)equipable);
            }
        }
    }

}
