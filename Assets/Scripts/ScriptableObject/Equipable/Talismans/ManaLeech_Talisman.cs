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
    }
    public override void OnFightEnd(Profile owner)
    {
        ownerCount--;
    }






    public void AbsorbMana(Profile owner, Profile spender, float amount)
    {
        TurnScheduler.AddAction(skill.Method(owner, spender, amount / ownerCount));
    }
}
