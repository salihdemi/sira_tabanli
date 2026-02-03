using UnityEngine;

public class Food : Useable
{
    public override void Method(Profile user, Profile target)
    {
        InventoryManager.RemoveFood(this);
    }
}
