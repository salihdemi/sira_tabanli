using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealAllAllies", menuName = "Scriptable Objects/Skills/CharacterSkills/HealAllAllies")]
public class HealAllAllies : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat


        //konsola yaz
        string text = user.stats._name + " " + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);
        

        //saldýrýyý yap
        foreach (Profile ally in FightManager.AllyProfiles)
        {
            ally.AddToHealth(5, user);
        }
        yield return new WaitForSeconds(1);
        
    }
}