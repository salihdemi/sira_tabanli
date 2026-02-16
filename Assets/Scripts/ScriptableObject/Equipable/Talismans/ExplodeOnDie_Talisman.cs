using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/ExplodeOnDie_Talisman")]
public class ExplodeOnDie_Talisman : Talisman
{
    public float dieDamage = 5f;
    public float reflectDamage = 10f;
    //sadece dostlara da vurabilir
    public override void OnTakeDamage(Profile owner, float damage)
    {
        foreach (Profile profile in TurnScheduler.GetAliveProfiles())
        {
            profile.AddToHealth(-reflectDamage, null);
        }
    }

    public override void OnDie(Profile owner, float damage)
    {
        foreach (Profile profile in TurnScheduler.GetAliveProfiles())
        {
            profile.AddToHealth(-dieDamage, null);
        }
    }
}