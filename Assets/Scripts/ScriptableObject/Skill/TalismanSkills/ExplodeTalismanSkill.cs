using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeTalismanSkill", menuName = "Scriptable Objects/Skills/TalismanSkills/ExplodeTalismanSkill")]
public class ExplodeTalismanSkill : Skill
{
    public override IEnumerator Method(Profile user, Profile target, float damage)
    {
        //animasyonu oynat
        //sesi oynat


        //konsola yazma, cagirilirken yaziliyor
        string text = $"{user.stats._name} patlayarak tepki verdi!";
        ConsolePanel.instance.WriteConsole(text);

        //saldýrýyý yap
        ProfileLungeHandler[] profiles = TurnScheduler.orderedProfiles.ToArray();//sadece allylara da vurabilir
        foreach (ProfileLungeHandler lungeHandler in profiles)
        {
            if (lungeHandler != null && lungeHandler != user && !lungeHandler.profile.stats.isDied)
            {
                lungeHandler.profile.AddToHealth(-damage, null);
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
