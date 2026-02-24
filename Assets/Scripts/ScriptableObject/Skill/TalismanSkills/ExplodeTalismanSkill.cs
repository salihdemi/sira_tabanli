using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeTalismanSkill", menuName = "Scriptable Objects/Skills/TalismanSkills/ExplodeTalismanSkill")]
public class ExplodeTalismanSkill : TalismanSkill
{
    public override IEnumerator Method(Profile user, Profile target, float damage)
    {
        //animasyonu oynat
        //sesi oynat


        //konsola yazma, cagirilirken yaziliyor
        string log = $"{user.stats._name} patlayarak tepki verdi!";
        ConsolePanel.instance.WriteConsole(log);

        //saldýrýyý yap
        Profile[] profiles = TurnScheduler.GetAliveProfiles().ToArray();//sadece allylara da vurabilir
        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != user && !profile.isDied)
            {
                profile.AddToHealth(-damage, null);
            }
        }

        //beklet
        yield return new WaitForSeconds(1f); // 1 saniye bekle
    }
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;
    }
}
