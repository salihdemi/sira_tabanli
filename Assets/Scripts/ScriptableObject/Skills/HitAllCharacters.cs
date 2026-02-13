using UnityEngine;

[CreateAssetMenu(fileName = "HitAllCharacters", menuName = "Scriptable Objects/Useables/Skills/HitAllCharacters")]
public class HitAllCharacters : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        foreach (AllyProfile profile in TurnScheduler.GetAliveProfiles())
        {
            profile.AddToHealth(-5, user);
        }
    }
}