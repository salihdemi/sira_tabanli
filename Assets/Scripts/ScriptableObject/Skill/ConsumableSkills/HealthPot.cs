using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPot", menuName = "Scriptable Objects/Skills/ConsumableSkills/HealthPot")]
public class HealthPot : ConsumableSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        user.AddToHealth(10, user);
        InventoryManager.RemoveConsumable(this);
    }
}
