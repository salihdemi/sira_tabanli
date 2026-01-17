

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    public static Profile defaultTargetForEnemies;//!

    [SerializeField] private BattleSpawner battleSpawner;
    [SerializeField] public TurnScheduler turnScheduler;
    [SerializeField] private PartyManager  partyManager;

    [SerializeField] private GameObject fightPanel;





    public static event Action OnFightStart, OnFightEnd;


    private string fightLoot;
    private void Awake()
    {
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable += StartFight;
    }
    private void OnDestroy()
    {
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable -= StartFight;
    }







    public void StartFight(EnemyMoveable enemy)//fonksiyonla
    {
        #region NullCheck
        if (partyManager.party.Length < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemy.enemyStats.Length < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        OnFightStart.Invoke();

        fightPanel.SetActive(true);

        fightLoot = enemy.loot;


        PersistanceStats[] allyStats = partyManager.party;
        List<AllyProfile> ActiveAllyProfiles = battleSpawner.SpawnAllies(allyStats);

        PersistanceStats[] enemyStats = enemy.enemyStats;
        List<EnemyProfile> ActiveEnemyProfiles = battleSpawner.SpawnEnemies(enemyStats);


        defaultTargetForEnemies = ActiveAllyProfiles[0];//!

        turnScheduler.SetAliveProfiles(ActiveAllyProfiles, ActiveEnemyProfiles);
        turnScheduler.SortProfilesWithSpeed();

        //battleSpawner.ResetStats(AllyProfiles);

        turnScheduler.StartTour();
    }
    
    public void WinFight()
    {
        //Ödül ver
        Debug.Log(fightLoot + "kazanildi");

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

        fightPanel.SetActive(false);
    }













}
