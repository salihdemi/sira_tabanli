using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Overlays;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UI;

public static class BattleSpawner
{





    //bu 2 fonksiyon neredeyse ayný
    private static Profile MakeAllyProfile(PersistanceStats persistanceStats)
    {
        ProfileLungeHandler lungeHandler = FightPanelObjectPool.instance.GetAlly();

        Profile oldProfile = lungeHandler.GetComponent<Profile>();//eski profile kaldýysa
        if (oldProfile != null) GameObject.Destroy(oldProfile);//sil

        // 3. Karar Verme: Stats içindeki tipe göre yeni scripti ekle
        Profile finalProfile;
        switch (persistanceStats.type)
        {
            case CharacterType.a:
                finalProfile = lungeHandler.gameObject.AddComponent<DefaultAllyProfile>();
                break;

            case CharacterType.b:
                finalProfile = lungeHandler.gameObject.AddComponent<DefaultEnemyProfile>();
                break;

            default:
                Debug.LogWarning($"{lungeHandler.gameObject} için profil oluþturulamadý! Tip eþleþmiyor olabilir.");
                return null;
        }

        ProfileButtonHandler buttonHandler = lungeHandler.GetComponent<ProfileButtonHandler>();
        ProfileUIHandler UIHandler = lungeHandler.GetComponent<ProfileUIHandler>();

        lungeHandler.profile = finalProfile;
        lungeHandler.buttonHandler = buttonHandler;

        buttonHandler.profile = finalProfile;

        UIHandler.profile = finalProfile;

        finalProfile.lungeHandler = lungeHandler;
        finalProfile.isAlly = false;
        finalProfile.Setup(persistanceStats);

        return finalProfile;

    }
    private static Profile MakeEnemyProfile(PersistanceStats persistanceStats)
    {
        EnemyProfileLungeHandler lungeHandler = FightPanelObjectPool.instance.GetEnemy();

        Profile oldProfile = lungeHandler.GetComponent<Profile>();//eski profile kaldýysa
        if (oldProfile != null) GameObject.Destroy(oldProfile);//sil

        // 3. Karar Verme: Stats içindeki tipe göre yeni scripti ekle
        Profile finalProfile;
        switch (persistanceStats.type)
        {
            case CharacterType.a:
                finalProfile = lungeHandler.gameObject.AddComponent<DefaultAllyProfile>();
                break;

            case CharacterType.b:
                finalProfile = lungeHandler.gameObject.AddComponent<DefaultEnemyProfile>();
                break;

            default:
                Debug.LogWarning($"{lungeHandler.gameObject} için profil oluþturulamadý! Tip eþleþmiyor olabilir.");
                return null;
        }

        ProfileButtonHandler buttonHandler = lungeHandler.GetComponent<ProfileButtonHandler>();
        ProfileUIHandler UIHandler = lungeHandler.GetComponent<ProfileUIHandler>();

        lungeHandler.profile = finalProfile;
        lungeHandler.buttonHandler = buttonHandler;

        buttonHandler.profile = finalProfile;

        UIHandler.profile = finalProfile;

        finalProfile.lungeHandler = lungeHandler;
        finalProfile.isAlly = false;
        finalProfile.Setup(persistanceStats);

        return finalProfile;
    }




    public static List<Profile> SpawnAllies(List<PersistanceStats> partyStats)
    {
        List<Profile> allyProfiles = new List<Profile>();

        for (int i = 0; i < partyStats.Count; i++)
        {
            if (!partyStats[i].isDied)
            {
                Profile spawnedProfile = MakeAllyProfile(partyStats[i]);

                // !!! NULL CHECK BURADA !!!
                if (spawnedProfile) allyProfiles.Add(spawnedProfile);
            }
        }
        return allyProfiles;
    }
    public static List<Profile> SpawnEnemies(List<PersistanceStats> partyStats)
    {
        List<Profile> enemyProfiles = new List<Profile>();

        for (int i = 0; i < partyStats.Count; i++)
        {
            if (!partyStats[i].isDied)
            {
                Profile spawnedProfile = MakeEnemyProfile(partyStats[i]);

                // !!! NULL CHECK BURADA !!!
                if (spawnedProfile) enemyProfiles.Add(spawnedProfile);
            }
        }
        return enemyProfiles;
    }
    public static void ClearBattlefield()
    {
        FightPanelObjectPool.instance.ClearAllies();
        FightPanelObjectPool.instance.ClearEnemies();
    }





}
