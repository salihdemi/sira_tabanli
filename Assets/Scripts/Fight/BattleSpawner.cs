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
        AllyProfile profile = FightPanelObjectPool.instance.GetAlly();

        profile.Setup(persistanceStats);

        return profile;
    }
    private static EnemyProfile MakeEnemyProfile(PersistanceStats persistanceStats)
    {
        EnemyProfile profile = FightPanelObjectPool.instance.GetEnemy();

        profile.Setup(persistanceStats);

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
        FightPanelObjectPool.instance.ClearAllies();
        FightPanelObjectPool.instance.ClearEnemies();
    }





}
