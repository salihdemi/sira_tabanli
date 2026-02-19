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
        Debug.Log("ontakeDamage");
        TurnScheduler.Something(1, () => HitAll(owner, damage));
    }

    public override void OnDie(Profile owner, Profile dealer, float damage)
    {
        TurnScheduler.Something(1, () => HitAll(owner, damage));
    }

    private void HitAll(Profile owner, float damage)
    {
        Profile[] profiles = TurnScheduler.GetAliveProfiles().ToArray();

        string log = owner + " patlayarak herkese" + damage + " hasar vurdu";
        ConsolePanel.instance.WriteConsole(log);

        foreach (Profile profile in profiles)
        {
            if (profile != null && profile != owner && !profile.isDied)
            {
                profile.AddToHealth(-damage, null);
            }
        }



    }
}