using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBehaviourSet : ScriptableObject
{

    //KRITER

    //en tehlikeli?
    //scripted
    //liste usülü
    //support olan


    //yetenek ardarda vurmama

    //aðýrlýklý rastsantýsallýk


    //ISLEM

    //vurmma
    //susturma
    //statü verme
    //iyileþtirme





    // HER DAVRANIÞ BU METODU DOLDURACAK
    public virtual void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        ChooseSkill(lungeHandler, GetRandomUsableSkill(lungeHandler.profile));
        ChooseTarget(lungeHandler, GetRandomAlly());
    }






    #region Kontrol etme
    protected bool IsHealthEnough(Profile profile, float percent)
    {
        PersistanceStats stats = profile.stats;

        float healthPercent = stats.currentHealth / stats.maxHealth;

        return healthPercent > percent / 100;
    }

    protected bool IsManaEnough(Profile profile, float percent)
    {
        PersistanceStats stats = profile.stats;

        float manaPercent = stats.currentMana / stats.maxMana;

        return manaPercent > percent / 100;
    }
    protected bool IsEnuoghForSkill(Profile profile, Skill skill)
    {
        return profile.IsEnoughForSkill(skill);
    }
    protected bool IsBurning(Profile profile)
    {
        return profile.fire > 0;
    }
    #endregion




    
    protected void ChooseTarget(ProfileLungeHandler lungeHandler, Profile target)
    {
        lungeHandler.ChooseTarget(target);
    }
    #region Þarta göre profil bulma
    protected Profile ReturnTargetByTargetType(TargetType targetType, Profile profile)
    {
        Profile target;
        switch (targetType)
        {
            case TargetType.enemy:
                target = GetRandomEnemy();
                break;

            case TargetType.ally:
                target = GetRandomAlly();
                break;

            case TargetType.self:
                target = profile;
                break;

            default:
                target = null;
                break;
        }

        return target;
    }

    protected Profile GetRandomAlly() =>
        FightManager.AllyProfiles.Where(p => !p.stats.isDied).OrderBy(x => Random.value).FirstOrDefault();

    protected Profile GetRandomEnemy() =>
        FightManager.EnemyProfiles.Where(p => !p.stats.isDied).OrderBy(x => Random.value).FirstOrDefault();

    protected Profile GetLowestHealthAlly() =>
        FightManager.AllyProfiles.Where(p => !p.stats.isDied).OrderBy(p => p.stats.currentHealth).FirstOrDefault();

    protected Profile GetLowestHealthEnemy() =>
        FightManager.EnemyProfiles.Where(p => !p.stats.isDied).OrderBy(p => p.stats.currentHealth).FirstOrDefault();

    protected Profile GetHighestStrengthAlly() =>
        FightManager.AllyProfiles.Where(p => !p.stats.isDied).OrderBy(p => p.currentStrength).FirstOrDefault();

    protected Profile GetHighestStrengthEnemy() =>
        FightManager.EnemyProfiles.Where(p => !p.stats.isDied).OrderBy(p => p.currentStrength).FirstOrDefault();

    protected Profile GetRandomBurningEnemy() =>
        FightManager.EnemyProfiles.Where(p => !p.stats.isDied &&p.fire > 0).OrderBy(p => Random.value).FirstOrDefault();

    protected Profile GetRandomBurningAllyy() =>
        FightManager.AllyProfiles.Where(p => !p.stats.isDied && p.fire > 0).OrderBy(p => Random.value).FirstOrDefault();






    protected Profile GetLastAttacker(Profile profile)
    {
        return profile.lastAttacker;//nullchecklenmeli
    }
    #endregion





    protected void ChooseSkill(ProfileLungeHandler lungeHandler, Skill skill)
    {
        lungeHandler.ChooseSkill(skill);
    }
    #region Skill seçme
    protected Skill GetRandomSkill(Profile profile)
    {
        int a = profile.stats.currentSkills.Count;
        int b = Random.Range(0, a);

        return profile.stats.currentSkills[b];
    }

    protected Skill GetRandomUsableSkill(Profile profile)
    {
        // Sadece enerjinin yettiði yetenekler + temel saldýrý
        List<Skill> usableSkills = new List<Skill>(profile.stats.currentSkills) { profile.stats.attack };

        return usableSkills[Random.Range(0, usableSkills.Count)];
    }
    #endregion
}