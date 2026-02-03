using UnityEngine;

public class Food : Consumable
{
    public override void Method(Profile user, Profile target)
    {
        InventoryManager.instance.DecreaseFood(this);
    }
}
