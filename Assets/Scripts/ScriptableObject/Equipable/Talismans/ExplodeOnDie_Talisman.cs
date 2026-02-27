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

    public Skill explode;
    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        if (!owner.stats.isDied)
        {
            TurnScheduler.AddAction(explode.Method(owner, null, reflectDamage));
        }
    }

    public override void OnDie(Profile owner, Profile dealer, float damage)
    {
        TurnScheduler.AddAction(explode.Method(owner, null, dieDamage));
    }

}