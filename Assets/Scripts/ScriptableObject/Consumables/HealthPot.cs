using UnityEngine;

[CreateAssetMenu(fileName = "HealthPot", menuName = "Scriptable Objects/Useables/Consumables/HealthPot")]
public class HealthPot : Consumable
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        user.AddToHealth(10);
        InventoryManager.RemoveConsumable(this);
    }
}
