using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/Thorns")]
public class Thorns_Talisman : Talisman
{
    public float reflectDamage = 5f;

    public Skill skill;
    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        TurnScheduler.AddAction(skill.Method(owner, dealer, reflectDamage));
    }
}