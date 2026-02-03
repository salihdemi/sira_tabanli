using UnityEditor.SceneManagement;
using UnityEngine;

public class ColliderThing : MonoBehaviour
{
    public enum Type
    {
        characterUnlocker,
        foodGiver,
        toyGiver
    }
    public Type type;

    public CharacterData characterToUnlock;
    public Food foodToGive;
    public Toy toyToGive;


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
    }

}
