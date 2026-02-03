using UnityEngine;

public class Toy : Useable
{
    public override void Method(Profile user, Profile target)
    {
        InventoryManager.RemoveToy(this);
    }
}
