
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1Skill", menuName = "Scriptable Objects/Skills/ItemSkills/Item1Skill")]
public class Item1Skill : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        //animasyonu oynat
        //sesi oynat



        //konsola yaz
        string text = user.stats._name + " " + target.stats._name + "'e " + _name + " yapýyor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);

        yield return new WaitForSeconds(1);//!

        //saldýrýyý yap

        target.AddToHealth(-user.currentStrength, user);
        Debug.Log("item kullanýldý");
    }
}
