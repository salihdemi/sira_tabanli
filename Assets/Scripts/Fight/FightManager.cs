

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
    public static Profile tauntedAlly;
    public static Profile tauntedEnemy;

    public static Profile defaultTargetForEnemies;//!

    public static void SetDefaultTarget()//gecici
    {
        if (TurnScheduler.ActiveAllyProfiles.Count > 0)
        {
            defaultTargetForEnemies = TurnScheduler.ActiveAllyProfiles[0].profile;//!
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
        List<Profile> ActiveAllyProfiles = BattleSpawner.SpawnAllies(PartyManager.partyStats);

        //spawn enemies
        List<Profile> ActiveEnemyProfiles = BattleSpawner.SpawnEnemies(enemy.enemyStats);



        TurnScheduler.SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        TurnScheduler.SortProfilesWithSpeed();

        //battleSpawner.ResetStats(AllyProfiles);

        SetDefaultTarget();//!

        foreach (Profile profile in ActiveAllyProfiles) profile.stats.talimsan?.OnFightStart(profile);
        foreach (Profile profile in ActiveEnemyProfiles) profile.stats.talimsan?.OnFightStart(profile);
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
        foreach (ProfileLungeHandler lungeHandler in TurnScheduler.ActiveAllyProfiles) lungeHandler.profile.stats.talimsan?.OnFightEnd(lungeHandler.profile);

        OnFightEnd.Invoke();//moveable-setisinfight

        BattleSpawner.ClearBattlefield();

        defaultTargetForEnemies = null;

        fightPanel.SetActive(false);
    }













}
