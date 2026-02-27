using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/ManaLeech_Talisman")]
public class ManaLeech_Talisman : Talisman
{
    public Skill skill;


    private static int ownerCount;
    public override void OnFightStart(Profile owner)
    {
        ownerCount++;
    }
    public override void OnDie(Profile owner, Profile dealer, float damage)
    {
        ownerCount--;
    }
    public override void OnFightEnd(Profile owner)
    {
        ownerCount = 0;
    }






    public void AbsorbMana(Profile owner, Profile spender, float amount)
    {
        TurnScheduler.AddAction(skill.Method(owner, spender, amount / ownerCount));
    }




}
