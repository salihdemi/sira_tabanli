using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/ExplodeOnDie_Talisman")]
public class ExplodeOnDie_Talisman : Talisman
{
    public float reflectDamage = 5f;
    public float dieDamage = 10f;
    //sadece allylara da vurabilir
    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        if (!owner.isDied)
        {
            string log = $"{owner.name} patlayarak tepki verdi!";
            ConsolePanel.instance.WriteConsole(log);
            CombatManager.AddAction(TalismanEffectRoutine(owner, damage));
        }
    }

    public override void OnDie(Profile owner, Profile dealer, float damage)
    {
        string log = $"{owner.name} ölüm hasarý verdi!";
        ConsolePanel.instance.WriteConsole(log);
        CombatManager.AddAction(TalismanEffectRoutine(owner, damage));
    }
    private IEnumerator TalismanEffectRoutine(Profile owner, float damage)
    {
        HitAll(owner, damage);
        yield return new WaitForSeconds(1f); // 1 saniye bekle
    }
    private void HitAll(Profile owner, float damage)
    {
        string log = $"{owner.name} patlayarak tepki verdi!";

        ConsolePanel.instance.WriteConsole(log);

        Profile[] profiles = TurnScheduler.GetAliveProfiles().ToArray();
        
        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != owner && !profile.isDied)
            {
                profile.AddToHealth(-damage, null);
            }
        }

    }
}