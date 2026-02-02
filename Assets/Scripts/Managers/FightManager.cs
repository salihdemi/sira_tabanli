

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    //düþman kendi hedefini seçebilince silinecek
    public static FightManager instance;
    public static Profile defaultTargetForEnemies;//!

    public void SetDefaultTarget()//gecici
    {
        if (TurnScheduler.ActiveAllyProfiles.Count > 0)
        {
            defaultTargetForEnemies = TurnScheduler.ActiveAllyProfiles[0];//!
        }
    }

    //-----------------------







    [SerializeField] private PartyManager  partyManager;

    [SerializeField] private GameObject fightPanel;





    public static event Action OnFightStart, OnFightEnd;

    private EnemyGroup currentEnemy;

    private void Awake()
    {
        instance = this;
        EnemyGroup.OnSomeoneCollideMainCharacterMoveable += StartFight;
    }
    private void OnDestroy()
    {
        EnemyGroup.OnSomeoneCollideMainCharacterMoveable -= StartFight;
    }







    public void StartFight(EnemyGroup enemy)//fonksiyonla
    {
        #region NullCheck
        if (partyManager.partyStats.Count < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemy.enemyStats.Count < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        OnFightStart.Invoke();

        fightPanel.SetActive(true);

        currentEnemy = enemy;


        List<PersistanceStats> allyStats = partyManager.partyStats;
        List<AllyProfile> ActiveAllyProfiles = BattleSpawner.SpawnAllies(allyStats);

        List<PersistanceStats> enemyStats = enemy.enemyStats;
        List<EnemyProfile> ActiveEnemyProfiles = BattleSpawner.SpawnEnemies(enemyStats);



        TurnScheduler.SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        TurnScheduler.SortProfilesWithSpeed();

        //battleSpawner.ResetStats(AllyProfiles);

        SetDefaultTarget();//!

        TurnScheduler.StartTour();
    }
    
    public void WinFight()
    {
        //Ödül ver
        Debug.Log(currentEnemy.loot + "kazanildi");


        currentEnemy.LoseFight();


        //resetstats!
        foreach (Profile item in TurnScheduler.ActiveAllyProfiles)
        {
            item.ResetStats();
        }

        FinishFight();
    }
    public void LoseFight()
    {
        Debug.Log("lose");
        //ölüm ekraný* vs
        SceneManager.LoadScene(SceneManager.loadedSceneCount);//save sistemi degisince

        FinishFight();
    }
    
    public void FinishFight()
    {
        OnFightEnd.Invoke();//moveable-setisinfight

        BattleSpawner.ClearBattlefield();

        defaultTargetForEnemies = null;

        fightPanel.SetActive(false);
    }













}
