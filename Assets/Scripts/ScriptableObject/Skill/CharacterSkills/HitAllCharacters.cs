using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllCharacters", menuName = "Scriptable Objects/Useables/Skills/HitAllCharacters")]
public class HitAllCharacters : CharacterSkill
{
    public float damage;
    public override IEnumerator Method(Profile user, Profile target)
    {
        //animasyonu oynat
        //sesi oynat


        //konsola yaz
        string log = $"{user.stats._name} patladý!";
        ConsolePanel.instance.WriteConsole(log);

        //saldýrýyý yap
        Profile[] profiles = TurnScheduler.GetAliveProfiles().ToArray();
        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != user && !profile.isDied)
            {
                profile.AddToHealth(-damage, null);
            }
        }

        //beklet
        yield return new WaitForSeconds(1f); // 1 saniye bekle
    }
}