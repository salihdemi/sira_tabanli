using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HitAllEnemies", menuName = "Scriptable Objects/Skills/CharacterSkills/HitAllEnemies")]
public class HitAllEnemies : CharacterSkill
{
    public override IEnumerator Method(Profile user, Profile target)
    {
        yield return null;//!
        //animasyonu oynat
        //sesi oynat

        //saldýrýyý yap
        /*
        foreach (EnemyProfileLungeHandler enemy in TurnScheduler.ActiveEnemyProfiles)
        {
            enemy.profile.AddToHealth(-5, user);
        }*/
    }
}
