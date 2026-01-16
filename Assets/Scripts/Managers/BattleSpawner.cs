using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpawner : MonoBehaviour
{

    [SerializeField] private ObjectPoolManager objectPoolManager;

    [Header("Profiles")]
    [SerializeField] private AllyProfile AllyProfilePrefab;
    [SerializeField] private EnemyProfile EnemyProfilePrefab;

    [SerializeField] private Transform AllyProfileParent;
    [SerializeField] private Transform EnemyProfileParent;




    private AllyProfile MakeAllyProfile(AllyData data)
    {
        //AllyProfile profile = Instantiate(AllyProfilePrefab, AllyProfileParent);
        AllyProfile profile = objectPoolManager.GetAlly();
        profile.BaseData = data;
        profile.gameObject.name = data.name;
        profile.GetComponent<Image>().sprite = data._sprite;

        profile.ChangeHealth(data.maxHealth);
        profile.ResetStats();
        //deger yazma
        return profile;
    }
    private EnemyProfile MakeEnemyProfile(EnemyData data)
    {
        //EnemyProfile profile = Instantiate(EnemyProfilePrefab, EnemyProfileParent);
        EnemyProfile profile = objectPoolManager.GetEnemy();
        profile.BaseData = data;
        profile.gameObject.name = data.name;
        profile.GetComponent<Image>().sprite = data._sprite;
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
    



    public List<AllyProfile> SpawnAllies(AllyData[] party)
    {
        List<AllyProfile> allyProfiles = new List<AllyProfile> { };


        for (int i = 0; i < party.Length; i++)
        {
            allyProfiles.Add(MakeAllyProfile(party[i]));
        }
        return allyProfiles;

    }
    public List<EnemyProfile> SpawnEnemies(EnemyData[] enemies)
    {
        List<EnemyProfile> enemyProfiles = new List<EnemyProfile> { };



        for (int i = 0; i < enemies.Length; i++)
        {
            enemyProfiles.Add(MakeEnemyProfile(enemies[i]));
        }
        return enemyProfiles;
    }
    public void ClearBattlefield()
    {
        objectPoolManager.ClearAllies();
        objectPoolManager.ClearEnemies();
    }





}
