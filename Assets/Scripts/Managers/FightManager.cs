

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
        if (turnScheduler.ActiveAllyProfiles.Count > 0)
        {
            defaultTargetForEnemies = turnScheduler.ActiveAllyProfiles[0];//!
        }
    }

    //-----------------------







    [SerializeField] private BattleSpawner battleSpawner;
    [SerializeField] public TurnScheduler turnScheduler;
    [SerializeField] private PartyManager  partyManager;

    [SerializeField] private GameObject fightPanel;





    public static event Action OnFightStart, OnFightEnd;


    private string fightLoot;
    private void Awake()
    {
        instance = this;
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable += StartFight;
    }
    private void OnDestroy()
    {
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable -= StartFight;
    }







    public void StartFight(EnemyMoveable enemy)//fonksiyonla
    {
        #region NullCheck
        if (partyManager.partyStats.Length < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemy.enemyStats.Length < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        OnFightStart.Invoke();

        fightPanel.SetActive(true);

        fightLoot = enemy.loot;


        PersistanceStats[] allyStats = partyManager.partyStats;
        List<AllyProfile> ActiveAllyProfiles = battleSpawner.SpawnAllies(allyStats);

        PersistanceStats[] enemyStats = enemy.enemyStats;
        List<EnemyProfile> ActiveEnemyProfiles = battleSpawner.SpawnEnemies(enemyStats);



        turnScheduler.SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        turnScheduler.SortProfilesWithSpeed();

        //battleSpawner.ResetStats(AllyProfiles);

        SetDefaultTarget();//!

        turnScheduler.StartTour();
    }
    
    public void WinFight()
    {
        //Ödül ver
        Debug.Log(fightLoot + "kazanildi");



        //resetstats!

        FinishFight();
    }
    public void LoseFight()
    {
        Debug.Log("lose");
        //ölüm ekraný* vs
        SceneManager.LoadScene(0);//save sistemi degisince

        FinishFight();
    }
    
    public void FinishFight()
    {
        OnFightEnd.Invoke();//moveable-setisinfight

        battleSpawner.ClearBattlefield();

        defaultTargetForEnemies = null;

        fightPanel.SetActive(false);
    }













}
