using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Taunt", menuName = "Scriptable Objects/Skills/CharacterSkills/Taunt")]
public class Taunt : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        //animasyonu oynat
        //sesi oynat

        //Konsola yaz
        string text = "";
        if (targetType == TargetType.self) text = user.stats._name + " taunt yapýyor";
        else text = user.stats._name + " " + target.stats._name + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);

        yield return new WaitForSeconds(1f);
        target.Taunt();
    }
}
