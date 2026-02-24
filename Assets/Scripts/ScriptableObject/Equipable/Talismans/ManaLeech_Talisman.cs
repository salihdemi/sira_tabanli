using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/ManaLeech_Talisman")]
public class ManaLeech_Talisman : Talisman
{
    public TalismanSkill skill;


    private static int ownerCount;
    public override void OnFightStart(Profile owner)
    {
        ownerCount++;
        Profile.OnAnyManaConsumed += HandleGlobalManaConsumption;
    }
    public override void OnFightEnd(Profile owner)
    {
        ownerCount--;
        Profile.OnAnyManaConsumed -= HandleGlobalManaConsumption;
    }






    private void HandleGlobalManaConsumption(Profile spender, float amount)
    {
        //ownera erişmek gerek!
        //TurnScheduler.AddAction(skill.Method(owner, spender, amount / ownerCount));
    }
}
