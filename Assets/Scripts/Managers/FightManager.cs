

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{

    [SerializeField] private BattleSpawner battleSpawner;
    [SerializeField] public TurnScheduler turnScheduler;
    [SerializeField] private PartyManager  partyManager;

    [SerializeField] private GameObject fightPanel;





    public static event Action OnFightStart, OnFightEnd;


    private string fightLoot;
    private void Awake()
    {
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable += StartFight;
        turnScheduler.onAllAlliesDie += LoseFight;
        turnScheduler.onAllEnemiesDie += WinFight;
    }
    private void OnDestroy()
    {
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable -= StartFight;
        turnScheduler.onAllAlliesDie -= LoseFight;
        turnScheduler.onAllEnemiesDie -= WinFight;
    }







    public void StartFight(EnemyMoveable enemy)
    {
        #region NullCheck
        if (partyManager.party.Length < 1)
        { Debug.LogError("Parti boþ"); return; }
        if (enemy.enemyStats.Length < 1)
        { Debug.LogError("Düþman partisi boþ"); return; }
        #endregion

        OnFightStart.Invoke();//moveable

        fightPanel.SetActive(true);
        fightLoot = enemy.loot;

        PersistanceStats[] allyStats = partyManager.party;
        turnScheduler.ActiveAllyProfiles = battleSpawner.SpawnAllies(allyStats);

        PersistanceStats[] enemyStats = enemy.enemyStats;
        turnScheduler.ActiveEnemyProfiles = battleSpawner.SpawnEnemies(enemyStats);




        turnScheduler.SetAliveProfiles();
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
