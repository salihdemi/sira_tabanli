using UnityEngine;

[CreateAssetMenu(fileName = "HitAllEnemies", menuName = "Scriptable Objects/Useables/Skills/HitAllEnemies")]
public class HitAllEnemies : Skill
{
    public override void Method(Profile user, Profile target)
    {

        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        foreach (EnemyProfile enemy in TurnScheduler.ActiveEnemyProfiles)
        {
            enemy.AddToHealth(-5, user);
        }
    }
}
