using UnityEditor.SceneManagement;
using UnityEngine;

public class ColliderThing : MonoBehaviour
{
    public enum Type
    {
        characterUnlocker,
        consumableGiver,
        equipableGiver
    }
    public Type type;

    public CharacterData characterToUnlock;
    public Consumable foodToGive;
    public Equipable equipable;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(type == Type.characterUnlocker)
        {
            if (characterToUnlock == null) return;
            PartyManager.UnlockAlly(characterToUnlock);
            Destroy(this);
        }
        else if (type == Type.consumableGiver)
        {
            InventoryManager.AddConsumable(foodToGive);
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
