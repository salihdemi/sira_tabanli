

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
    [SerializeField] private TurnScheduler turnScheduler;
    [SerializeField] private PartyManager  partyManager;

    [SerializeField] private GameObject fightPanel;



    [Header("Profiles")]
    [HideInInspector] public List<AllyProfile>  ActiveAllyProfiles  = new List<AllyProfile>();
    [HideInInspector] public List<EnemyProfile> ActiveEnemyProfiles = new List<EnemyProfile>();


    public static event Action OnFightStart, OnFightEnd;


    private string fightLoot;
    private void Awake()
    {
        Profile.OnSomeoneDie += HandleProfileDeath;
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable += StartFight;
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneDie -= HandleProfileDeath;
        EnemyMoveable.OnSomeoneCollideMainCharacterMoveable -= StartFight;
    }







    public void StartFight(EnemyMoveable enemy)
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
        ActiveAllyProfiles = battleSpawner.SpawnAllies(allyStats);

        PersistanceStats[] enemyStats = enemy.enemyStats;
        ActiveEnemyProfiles = battleSpawner.SpawnEnemies(enemyStats);


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




    public void HandleProfileDeath(Profile deadProfile)
    {
        // Listelerden çýkar
             if (deadProfile is AllyProfile  ally)  ActiveAllyProfiles. Remove(ally);
        else if (deadProfile is EnemyProfile enemy) ActiveEnemyProfiles.Remove(enemy);

        turnScheduler.RemoveFromQueue(deadProfile);

        // Savaþ bitti mi kontrol et
             if (ActiveAllyProfiles .Count == 0) LoseFight();
        else if (ActiveEnemyProfiles.Count == 0) WinFight();
    }









}
