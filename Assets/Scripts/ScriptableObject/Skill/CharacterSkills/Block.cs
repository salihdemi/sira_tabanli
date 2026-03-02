using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Scriptable Objects/Skills/CharacterSkills/Block")]
public class Block : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
                          //animasyonu oynat
                          //sesi oynat

        //konsola yaz
        string text = user.stats._name + " " + target.stats._name + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);

        //saldýrýyý yap
        user.AddToHealth(user.currentStrength, user);
        Debug.Log(user.name + " " + user.name + "'i " + name + " ile " + user.currentStrength + " iyileþtirdi");
    }
}
