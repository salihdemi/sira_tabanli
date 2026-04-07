

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public static class FightManager
{
    public static List<Profile> AllyProfiles = new List<Profile>();
    public static List<Profile> EnemyProfiles = new List<Profile>();






    public static Profile tauntedAlly;
    public static Profile tauntedEnemy;

    //d��man kendi hedefini se�ebilince silinecek
    //public static Profile defaultTargetForEnemies;//!

    public static void SetDefaultTarget()//gecici
    {
        if (AllyProfiles.Count > 0)
        {
            //defaultTargetForEnemies = AllyProfiles[0];//!
        }
    }

    //-----------------------


    private static GameObject fightPanel;


    public static event Action OnFightStart, OnFightEnd;

    private static EnemyGroup currentEnemy;




    public static void StartFight(List<CharacterData> enemies)
    {
        List<PersistanceStats> enemyStats = new List<PersistanceStats>();
        foreach (CharacterData data in enemies)
        {
            PersistanceStats stat = new PersistanceStats();
            stat.LoadFromBase(data);
            enemyStats.Add(stat);
        }

        #region NullCheck
        if (PartyManager.partyStats.Count < 1)
        { Debug.LogError("Parti boş"); return; }
        if (enemyStats.Count < 1)
        { Debug.LogError("Düşman partisi boş"); return; }
        #endregion

        OnFightStart.Invoke();
        if (fightPanel == null) fightPanel = FightPanelObjectPool.instance.gameObject;
        fightPanel.SetActive(true);
        currentEnemy = null;
        AllyProfiles = BattleSpawner.SpawnAllies(PartyManager.partyStats);
        EnemyProfiles = BattleSpawner.SpawnEnemies(enemyStats);
        SetDefaultTarget();
        foreach (Profile profile in AllyProfiles) profile.stats.talimsan?.OnFightStart(profile);
        foreach (Profile profile in EnemyProfiles) profile.stats.talimsan?.OnFightStart(profile);
        TurnScheduler.StartTour();
    }

    public static void StartFight(EnemyGroup enemy)
    {
        #region NullCheck
        if (PartyManager.partyStats.Count < 1)
        { Debug.LogError("Parti bo�"); return; }
        if (enemy.enemyStats.Count < 1)
        { Debug.LogError("D��man partisi bo�"); return; }
        #endregion

        OnFightStart.Invoke();

        if(fightPanel == null) fightPanel = FightPanelObjectPool.instance.gameObject;//!
        fightPanel.SetActive(true);

        currentEnemy = enemy;

        //spawn allies
        AllyProfiles = BattleSpawner.SpawnAllies(PartyManager.partyStats);

        //spawn enemies
        EnemyProfiles = BattleSpawner.SpawnEnemies(enemy.enemyStats);




        //battleSpawner.ResetStats(AllyProfiles);

        SetDefaultTarget();//!

        foreach (Profile profile in AllyProfiles) profile.stats.talimsan?.OnFightStart(profile);
        foreach (Profile profile in EnemyProfiles) profile.stats.talimsan?.OnFightStart(profile);
        TurnScheduler.StartTour();
    }
    
    public static void WinFight()
    {
        //�d�l ver
        Debug.Log(currentEnemy.loot + "kazanildi");


        currentEnemy.LoseFight();

        /*Test et
        //resetstats!
        foreach (Profile item in TurnScheduler.ActiveAllyProfiles)
        {
            item.ResetStats();
        }
        */
        currentEnemy = null;
        FinishFight();
    }
    public static void LoseFight()
    {
        Debug.Log("lose");
        //�l�m ekran�* vs
        SceneManager.LoadScene(SceneManager.loadedSceneCount);//save sistemi degisince

        FinishFight();
    }
    
    public static void FinishFight()
    {
        foreach (Profile profile in AllyProfiles) profile.stats.talimsan?.OnFightEnd(profile);

        OnFightEnd.Invoke();//moveable-setisinfight

        BattleSpawner.ClearBattlefield();

        //defaultTargetForEnemies = null;

        fightPanel.SetActive(false);
    }








    public static void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden ��kar
        RemoveFromQueue(deadProfile);

        // Sava� bitti mi kontrol et
        if (AllyProfiles.Count == 0)
        {
            if (TurnScheduler.playCoroutine != null)
            {
                //oynat corosunu durdur
                //runner.StopCor(playCoroutine);
                TurnScheduler.playCoroutine = null;
            }

            FightManager.LoseFight();
        }
        else if (EnemyProfiles.Count == 0)
        {
            if (TurnScheduler.playCoroutine != null)
            {
                //runner.StopCor(playCoroutine);
                TurnScheduler.playCoroutine = null;
            }

            FightManager.WinFight();
        }
    }
    private static void RemoveFromQueue(Profile deadProfile)
    {
        if (AllyProfiles.Contains(deadProfile))
        {
            AllyProfiles.Remove(deadProfile);
        }
        else if (EnemyProfiles.Contains(deadProfile))
        {
            EnemyProfiles.Remove(deadProfile);
        }
    }













    //mana ceken dost desteklemiyor.
    public static void OnAllyConsumeMana(Profile user, float amount)
    {
        foreach (Profile enemy in EnemyProfiles)
        {
            if (enemy.stats?.talimsan is ManaLeech_Talisman talisman)
            {
                talisman.AbsorbMana(enemy, user, amount);
            }
        }
    }
}
