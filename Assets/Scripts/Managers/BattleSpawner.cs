using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public static class BattleSpawner
{






    private static AllyProfile MakeAllyProfile(PersistanceStats persistanceStats)
    {
        AllyProfile profile = ObjectPoolManager.instance.GetAlly();
        profile.stats = persistanceStats;
        profile.gameObject.name = persistanceStats._name;
        profile.GetComponent<Image>().sprite = persistanceStats.sprite;

        profile.SetHealth(persistanceStats.currentHealth);
        profile.ResetStats();
        //deger yazma
        return profile;
    }
    private static EnemyProfile MakeEnemyProfile(PersistanceStats persistanceStats)
    {
        EnemyProfile profile = ObjectPoolManager.instance.GetEnemy();
        profile.stats = persistanceStats;
        profile.gameObject.name = persistanceStats._name;
        profile.GetComponent<Image>().sprite = persistanceStats.sprite;

        profile.SetHealth(persistanceStats.maxHealth);
        profile.ResetStats();
        //deger yazma

        return profile;
    }
    



    public static List<AllyProfile> SpawnAllies(List<PersistanceStats> partyStats)
    {
        List<AllyProfile> allyProfiles = new List<AllyProfile> { };


        for (int i = 0; i < partyStats.Count; i++)
        {
            if (!partyStats[i].isDied)
            {
                allyProfiles.Add(MakeAllyProfile(partyStats[i]));
            }
        }
        return allyProfiles;

    }
    public static List<EnemyProfile> SpawnEnemies(List<PersistanceStats> enemyStats)
    {
        List<EnemyProfile> enemyProfiles = new List<EnemyProfile> { };


        for (int i = 0; i < enemyStats.Count; i++)
        {
            enemyProfiles.Add(MakeEnemyProfile(enemyStats[i]));
        }
        return enemyProfiles;
    }
    public static void ClearBattlefield()
    {
        ObjectPoolManager.instance.ClearAllies();
        ObjectPoolManager.instance.ClearEnemies();
    }





}
