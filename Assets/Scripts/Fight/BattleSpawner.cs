using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public static class BattleSpawner
{






    private static Profile MakeAllyProfile(PersistanceStats persistanceStats)
    {
        Profile profile = FightPanelObjectPool.instance.GetAlly();

        profile.Setup(persistanceStats);

        return profile;
    }
    private static Profile MakeEnemyProfile(PersistanceStats persistanceStats)
    {
        Profile profile = FightPanelObjectPool.instance.GetEnemy();

        profile.Setup(persistanceStats);

        return profile;
    }
    



    public static List<Profile> SpawnAllies(List<PersistanceStats> partyStats)
    {
        List<Profile> allyProfiles = new List<Profile> { };


        for (int i = 0; i < partyStats.Count; i++)
        {
            if (!partyStats[i].isDied)
            {
                allyProfiles.Add(MakeAllyProfile(partyStats[i]));
            }
        }
        return allyProfiles;

    }
    public static List<Profile> SpawnEnemies(List<PersistanceStats> enemyStats)
    {
        List<Profile> enemyProfiles = new List<Profile> { };


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
