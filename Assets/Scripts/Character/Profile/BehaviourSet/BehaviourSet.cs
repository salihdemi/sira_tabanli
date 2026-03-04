using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "DefaultBehaviour", menuName = "AI/Behaviours/Default")]
public class BehaviourSet : ScriptableObject
{

    //KRITER

    //dost
    //en tehlikeli?
    //scripted?
    //liste usülü


    //düþman
    //support olan
    //olumsuz etkisi olan
    //scripted
    //liste usülü





    //aðýrlýklý rastsantýsallýk


    //ISLEM

    //vurmma
    //susturma
    //statü verme
    //iyileþtirme





    // HER DAVRANIÞ BU METODU DOLDURACAK
    public virtual void DecideLunge(ProfileLungeHandler lungeHandler)
    {
        ChooseSkill(lungeHandler, lungeHandler.profile.stats.attack);
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
        var usableSkills = profile.stats.currentSkills
            .Concat(new[] { profile.stats.attack })
            .ToList();

        return usableSkills[Random.Range(0, usableSkills.Count)];
    }
    #endregion
}