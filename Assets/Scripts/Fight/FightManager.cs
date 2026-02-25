

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

    //düþman kendi hedefini seçebilince silinecek
    public static Profile defaultTargetForEnemies;//!

    public static void SetDefaultTarget()//gecici
    {
        if (AllyProfiles.Count > 0)
        {
            defaultTargetForEnemies = AllyProfiles[0];//!
        }
    }

    //-----------------------


    private static GameObject fightPanel;


    public static event Action OnFightStart, OnFightEnd;

    private static EnemyGroup currentEnemy;




    public static void StartFight(EnemyGroup enemy)//fonksiyonla
    {
        #region NullCheck
        if (PartyManager.partyStats.Count < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemy.enemyStats.Count < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
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
        TurnScheduler.StartTourLunges();
    }
    
    public static void WinFight()
    {
        //Ödül ver
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
        //ölüm ekraný* vs
        SceneManager.LoadScene(SceneManager.loadedSceneCount);//save sistemi degisince

        FinishFight();
    }
    
    public static void FinishFight()
    {
        foreach (Profile profile in AllyProfiles) profile.stats.talimsan?.OnFightEnd(profile);

        OnFightEnd.Invoke();//moveable-setisinfight

        BattleSpawner.ClearBattlefield();

        defaultTargetForEnemies = null;

        fightPanel.SetActive(false);
    }








    public static void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
        RemoveFromQueue(deadProfile);

        // Savaþ bitti mi kontrol et
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


}
