using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string ID;

    public enum Type { characterUnlocker, consumableGiver, equipableGiver }
    public Type type;

    public CharacterData characterToUnlock;
    public Consumable foodToGive;
    public Equipable equipable;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Give();
    }

    public void Give()
    {
        if (type == Type.characterUnlocker)
        {
            if (characterToUnlock == null) return;
            PartyManager.UnlockAlly(characterToUnlock);
        }
        else if (type == Type.consumableGiver)
        {
            InventoryManager.AddConsumable(foodToGive);
        }
        else if (type == Type.equipableGiver)
        {
            if (equipable is Weapon) InventoryManager.ownedWeapons.Add((Weapon)equipable);
            else if (equipable is Item) InventoryManager.ownedItems.Add((Item)equipable);
            else if (equipable is Talisman) InventoryManager.ownedTalismas.Add((Talisman)equipable);
        }

        SaveManager.RegisterCollected(ID);
        Destroy(gameObject);
    }
}
