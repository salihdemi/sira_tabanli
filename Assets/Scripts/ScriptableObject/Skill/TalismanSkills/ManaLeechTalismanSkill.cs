using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeTalismanSkill", menuName = "Scriptable Objects/Skills/TalismanSkills/ManaLeechTalismanSkill")]
public class ManaLeechTalismanSkill : TalismanSkill
{
    public override IEnumerator Method(Profile user, Profile target, float damage)
    {
        //animasyonu oynat
        //sesi oynat


        //konsola yaz
        string text = user.stats._name + target.stats._name + "'ten " + damage + " mana emdi";
        ConsolePanel.instance.WriteConsole(text);

        //saldýrýyý yap
        user.AddToMana(damage);

        //beklet
        yield return new WaitForSeconds(1f); // 1 saniye bekle
    }

    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;
    }
}
