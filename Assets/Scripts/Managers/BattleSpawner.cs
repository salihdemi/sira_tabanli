using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpawner : MonoBehaviour
{

    [SerializeField] private ObjectPoolManager objectPoolManager;

    [SerializeField] private AllyProfile AllyProfilePrefab;
    [SerializeField] private EnemyProfile EnemyProfilePrefab;

    [SerializeField] private Transform AllyProfileParent;
    [SerializeField] private Transform EnemyProfileParent;




    private AllyProfile MakeAllyProfile(PersistanceStats runtimeStats)
    {
        AllyProfile profile = objectPoolManager.GetAlly();
        profile.stats = runtimeStats;
        profile.gameObject.name = runtimeStats.originData.name;
        profile.GetComponent<Image>().sprite = runtimeStats.originData._sprite;

        profile.ChangeHealth(runtimeStats.maxHealth);
        profile.ResetStats();
        //deger yazma
        return profile;
    }
    private EnemyProfile MakeEnemyProfile(PersistanceStats runtimeStats)
    {
        EnemyProfile profile = objectPoolManager.GetEnemy();
        profile.stats = runtimeStats;
        profile.gameObject.name = runtimeStats.originData.name;
        profile.GetComponent<Image>().sprite = runtimeStats.originData._sprite;

        profile.ChangeHealth(runtimeStats.maxHealth);
        profile.ResetStats();
        //deger yazma

        return profile;
    }
    /*
    public void ResetStats(List<AllyProfile> AllyProfiles)
    {
        for (int i = 0; i < AllyProfiles.Count; i++)
        {
            Profile character = AllyProfiles[i];
            character.ResetStats();
        }
    }*/
    



    public List<AllyProfile> SpawnAllies(PersistanceStats[] partyStats)
    {
        List<AllyProfile> allyProfiles = new List<AllyProfile> { };


        for (int i = 0; i < partyStats.Length; i++)
        {
            allyProfiles.Add(MakeAllyProfile(partyStats[i]));
        }
        return allyProfiles;

    }
    public List<EnemyProfile> SpawnEnemies(PersistanceStats[] enemyStats)
    {
        List<EnemyProfile> enemyProfiles = new List<EnemyProfile> { };



        for (int i = 0; i < enemyStats.Length; i++)
        {
            enemyProfiles.Add(MakeEnemyProfile(enemyStats[i]));
        }
        return enemyProfiles;
    }
    public void ClearBattlefield()
    {
        objectPoolManager.ClearAllies();
        objectPoolManager.ClearEnemies();
    }





}
