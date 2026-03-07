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
        string text = user.stats._name + " " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);


        //saldýrýyý yap
        Profile[] profiles = FightManager.EnemyProfiles.ToArray();//sadece allylara da vurabilir
        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != user && !profile.stats.isDied)
            {
                //herkese ayrý mesaj?
                profile.AddToHealth(-user.currentStrength, null);
            }
        }
        yield return new WaitForSeconds(1f); // 1 saniye bekle
    }
}
