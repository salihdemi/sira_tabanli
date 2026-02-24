using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Skills/CharacterSkills/Attack")]
public class Attack : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat


        string text = user.stats._name + " " + target.stats._name + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);

        yield return new WaitForSeconds(1);

        //saldýrýyý yap
        target.AddToHealth(-user.currentStrength, user);
        //Debug.Log(user.name + " " + target.name + "'a " + name + " ile " + user.currentStrength + " hasar verdi");
    }
}
