using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpawner : MonoBehaviour
{
    //ins-destroy yerine enable-yaz yap

    [Header("Profiles")]
    [SerializeField] private AllyProfile AllyProfilePrefab;
    [SerializeField] private EnemyProfile EnemyProfilePrefab;

    [SerializeField] private Transform AllyProfileParent;
    [SerializeField] private Transform EnemyProfileParent;




    private AllyProfile MakeAllyProfile(AllyData data)
    {
        AllyProfile profile = Instantiate(AllyProfilePrefab, AllyProfileParent);
        profile.BaseData = data;
        profile.gameObject.name = data.name;
        profile.GetComponent<Image>().sprite = data._sprite;
        //deger yazma
        return profile;
    }
    private EnemyProfile MakeEnemyProfile(EnemyData data)
    {
        EnemyProfile profile = Instantiate(EnemyProfilePrefab, EnemyProfileParent);
        profile.BaseData = data;
        profile.gameObject.name = data.name;
        profile.GetComponent<Image>().sprite = data._sprite;
        //deger yazma

        return profile;
    }

    public void ResetStats(List<AllyProfile> AllyProfiles)
    {
        for (int i = 0; i < AllyProfiles.Count; i++)
        {
            Profile character = AllyProfiles[i];
            character.ResetStats();
        }
    }
    public void SetupProfile(Profile profile, CharacterBase data)
    {

        // Ölüm olayýna FightManager'ý abone et
        profile.onProfileDie += FightManager.instance.HandleProfileDeath;
        // Ölüm olayýna Scheduler'ý abone et
        profile.onProfileDie += FightManager.instance.turnScheduler.RemoveFromQueue;
    }



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
        foreach (Transform child in AllyProfileParent) Destroy(child.gameObject);
        foreach (Transform child in EnemyProfileParent) Destroy(child.gameObject);
    }





}
