using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealAllAllies", menuName = "Scriptable Objects/Skills/CharacterSkills/HealAllAllies")]
public class HealAllAllies : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        /*
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        foreach (Profile ally in FightManager.AllyProfiles)
        {
            ally.AddToHealth(5, user);
        }
        */
    }
}