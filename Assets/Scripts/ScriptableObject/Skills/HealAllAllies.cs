using UnityEngine;

[CreateAssetMenu(fileName = "HealAllAllies", menuName = "Scriptable Objects/Skills/HealAllAllies")]
public class HealAllAllies : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        foreach (AllyProfile enemy in TurnScheduler.ActiveAllyProfiles)
        {
            enemy.AddToHealth(5);
        }
    }
}