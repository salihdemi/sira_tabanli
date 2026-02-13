

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public static class FightManager
{
    //düþman kendi hedefini seçebilince silinecek
    public static Profile defaultTargetForEnemies;//!

    public static void SetDefaultTarget()//gecici
    {
        if (TurnScheduler.ActiveAllyProfiles.Count > 0)
        {
            defaultTargetForEnemies = TurnScheduler.ActiveAllyProfiles[0];//!
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


        List<PersistanceStats> allyStats = PartyManager.partyStats;
        List<AllyProfile> ActiveAllyProfiles = BattleSpawner.SpawnAllies(allyStats);

        List<PersistanceStats> enemyStats = enemy.enemyStats;
        List<EnemyProfile> ActiveEnemyProfiles = BattleSpawner.SpawnEnemies(enemyStats);



        TurnScheduler.SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        TurnScheduler.SortProfilesWithSpeed();

        //battleSpawner.ResetStats(AllyProfiles);

        SetDefaultTarget();//!

        TurnScheduler.StartTour();
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
        OnFightEnd.Invoke();//moveable-setisinfight

        BattleSpawner.ClearBattlefield();

        defaultTargetForEnemies = null;

        fightPanel.SetActive(false);
    }













}
