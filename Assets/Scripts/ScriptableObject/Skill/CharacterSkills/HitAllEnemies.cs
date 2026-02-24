using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllEnemies", menuName = "Scriptable Objects/Useables/Skills/HitAllEnemies")]
public class HitAllEnemies : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap

        foreach (EnemyProfile enemy in TurnScheduler.ActiveEnemyProfiles)
        {
            enemy.AddToHealth(-5, user);
        }
    }
}
