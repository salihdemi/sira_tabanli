using UnityEngine;

public class Toy : Consumable
{
    public override void Method(Profile user, Profile target)
    {
        InventoryManager.instance.DecreaseToy(this);
    }
}
