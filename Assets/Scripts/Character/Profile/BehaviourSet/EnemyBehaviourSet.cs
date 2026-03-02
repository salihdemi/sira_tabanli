using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public abstract class EnemyBehaviourSet : ScriptableObject
{


    // HER DAVRANIÞ BU METODU DOLDURACAK
    public virtual Skill DecideSkill(EnemyProfileLungeHandler lungeHandler)
    {
        return lungeHandler.profile.stats.attack; //default hamle
    }
    public virtual Profile DecideTarget(EnemyProfileLungeHandler lungeHandler, TargetType type)
    {
        return FightManager.defaultTargetForEnemies;
    }










    // Düþmanýn (Düþman gözüyle) rakiplerini (Bizim Ally'larý) bulur
    protected Profile GetRandomOpponent() =>
        FightManager.AllyProfiles.Where(p => !p.stats.isDied).OrderBy(x => Random.value).FirstOrDefault();

    protected Profile GetLowestHealthAlly() =>
        FightManager.AllyProfiles.Where(p => !p.stats.isDied).OrderBy(p => p.stats.currentHealth).FirstOrDefault();

    // Düþmanýn kendi takým arkadaþlarýný bulur (Support için)
    protected Profile GetLowestHealthEnemy() =>
        FightManager.EnemyProfiles.Where(p => !p.stats.isDied).OrderBy(p => p.stats.currentHealth).FirstOrDefault();





    protected Skill GetRandomSkill(EnemyProfileLungeHandler lungeHandler)
    {
        int a = lungeHandler.profile.stats.currentSkills.Count;
        int b = Random.Range(0, a);

        return lungeHandler.profile.stats.currentSkills[b];
    }
    protected bool IsEnuoghForSkill(EnemyProfileLungeHandler lungeHandler, Skill skill)
    {
        return lungeHandler.profile.IsEnoughForSkill(skill);
    }

}