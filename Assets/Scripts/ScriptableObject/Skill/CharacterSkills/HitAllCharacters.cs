using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllCharacters", menuName = "Scriptable Objects/Skills/CharacterSkills/HitAllCharacters")]
public class HitAllCharacters : Skill
{
    public float damage;
    public override IEnumerator Method(Profile user, Profile target)
    {
        
        //animasyonu oynat
        //sesi oynat



        //konsola yaz
        string text = user.stats._name + " " + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);


        //saldýrýyý yap
        Profile[] profiles = FightManager.AllyProfiles.ToArray();//sadece allylara da vurabilir
        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != user && !profile.stats.isDied)
            {
                profile.AddToHealth(-damage, null);
            }
        }
        yield return new WaitForSeconds(1f); // 1 saniye bekle
        
    }
}