using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/Thorns")]
public class Thorns_Talisman : Talisman
{
    public float reflectDamage = 5f;

    public TalismanSkill parry;
    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        CombatManager.AddAction(parry.Method(owner, dealer, reflectDamage));
    }




}