using NUnit.Framework;
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
        HitAll(owner, reflectDamage);
    }

    public override void OnDie(Profile owner, Profile dealer, float damage)
    {
        HitAll(owner, dieDamage);
    }

    private void HitAll(Profile owner, float damage)
    {
        Profile[] profiles = TurnScheduler.GetAliveProfiles().ToArray();

        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != owner && !profile.isDied)
            {
                profile.AddToHealth(-damage, null);
            }
        }
        owner.StartCoroutine(TurnScheduler.OnSomethingHappen(owner + " patlayarak herkese" + damage + " hasat vurdu", 1));
    }
}