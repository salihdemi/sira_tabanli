using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllEnemies", menuName = "Scriptable Objects/Skills/CharacterSkills/HitAllEnemies")]
public class HitAllEnemies : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
                          //animasyonu oynat
                          //sesi oynat


        //konsola yaz
        string text = user.stats._name + " " + target.stats._name + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);

        yield return new WaitForSeconds(1f); // 1 saniye bekle

        //saldýrýyý yap
        Profile[] profiles = FightManager.AllyProfiles.ToArray();//sadece allylara da vurabilir
        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != user && !profile.stats.isDied)
            {
                profile.AddToHealth(-5, null);
            }
        }
    }
}
