using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllCharacters", menuName = "Scriptable Objects/Skills/CharacterSkills/HitAllCharacters")]
public class HitAllCharacters : CharacterSkill
{
    public float damage;
    public override IEnumerator Method(Profile user, Profile target)
    {
        /*
        //animasyonu oynat
        //sesi oynat


        //konsola yaz
        string log = $"{user.stats._name} patladý!";
        ConsolePanel.instance.WriteConsole(log);

        //saldýrýyý yap
        ProfileLungeHandler[] profiles = FightManager.AllyProfiles.ToArray();//sadece allylara da vurabilir
        foreach (ProfileLungeHandler lungeHandler in profiles)
        {
            if (lungeHandler != null && lungeHandler.profile != user && !lungeHandler.profile.isDied)
            {
                lungeHandler.profile.AddToHealth(-damage, null);
            }
        }
        */
        //beklet
        yield return new WaitForSeconds(1f); // 1 saniye bekle
    }
}