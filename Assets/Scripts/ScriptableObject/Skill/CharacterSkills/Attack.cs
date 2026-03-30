using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Skills/CharacterSkills/Attack")]
public class Attack : Skill
{
    public override IEnumerator Method(Profile user, Profile target)
    {

        //saldırı animasyonu oynat
        PlayAnimation(user, "Attack");
        //hedefin hasar alma animasyonunu oynat
        PlayAnimation(target, "GetHit");
        //sesi oynat
        PlayAudio();
        //efekti oynat
        PlayEffect();

        //Konsol mesajı yaz
        string text = user.stats._name + " " + target.stats._name + "'e " + _name + " yap�yor";
        ConsolePanel.instance.WriteConsole(text);
        Debug.Log(text);

        //sald�r�y� yap
        target.AddToHealth(-user.currentStrength, user);

        yield return new WaitForSeconds(1);

        //Debug.Log(user.name + " " + target.name + "'a " + name + " ile " + user.currentStrength + " hasar verdi");
    }
}
