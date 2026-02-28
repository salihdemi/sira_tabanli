using System;
using System.Collections.Generic;
using UnityEngine;

public static class BattleSpawner
{
    // --- MERKEZÝ KURULUM (Ally ve Enemy için ortak mantýk) ---
    private static Profile ConfigureProfile(ProfileLungeHandler lungeHandler, PersistanceStats stats, bool isAlly)
    {
        // 1. Mevcut Component Kontrolü (Gereksiz Destroy'u önlemek için)
        Profile currentProfile = lungeHandler.GetComponent<Profile>();
        string targetTypeName = stats.type.ToString();
        Type targetType = Type.GetType(targetTypeName);

        if (targetType == null)
        {
            Debug.LogError($"{targetTypeName} isminde bir Profile sýnýfý bulunamadý!");
            return null;
        }

        // Eðer üzerindeki component zaten doðru tipteyse, SÝLME (Re-use)
        if (currentProfile != null && currentProfile.GetType() == targetType)
        {
            // Hiçbir þey silmeden sadece verileri tazele
            currentProfile.isAlly = isAlly;
            SetupConnections(lungeHandler, currentProfile);
            currentProfile.Setup(stats);
            return currentProfile;
        }

        // Eðer tip farklýysa veya hiç yoksa, eskiyi sil ve yenisini ekle
        if (currentProfile != null) GameObject.DestroyImmediate(currentProfile);

        Profile finalProfile = (Profile)lungeHandler.gameObject.AddComponent(targetType);

        // 2. Baðlantýlarý Kur (Dependency Injection)
        SetupConnections(lungeHandler, finalProfile);

        finalProfile.isAlly = isAlly;
        finalProfile.Setup(stats);

        return finalProfile;
    }

    // Baðlantýlarý kuran yardýmcý metod (DRY için dýþarý alýndý)
    private static void SetupConnections(ProfileLungeHandler lungeHandler, Profile profile)
    {
        ProfileButtonHandler buttonHandler = lungeHandler.GetComponent<ProfileButtonHandler>();
        ProfileUIHandler UIHandler = lungeHandler.GetComponent<ProfileUIHandler>();

        lungeHandler.profile = profile;
        lungeHandler.buttonHandler = buttonHandler;
        profile.lungeHandler = lungeHandler;

        if (buttonHandler != null) buttonHandler.profile = profile;
        if (UIHandler != null) UIHandler.profile = profile;
    }

    // --- SPAWN METODLARI ---

    public static List<Profile> SpawnAllies(List<PersistanceStats> partyStats)
    {
        List<Profile> allyProfiles = new List<Profile>();

        foreach (PersistanceStats stats in partyStats)
        {
            if (!stats.isDied)
            {
                // Pool'dan Ally handler alýyoruz
                ProfileLungeHandler lungeHandler = FightPanelObjectPool.instance.GetAlly();
                Profile spawned = ConfigureProfile(lungeHandler, stats, true);

                if (spawned != null) allyProfiles.Add(spawned);
            }
        }
        return allyProfiles;
    }

    public static List<Profile> SpawnEnemies(List<PersistanceStats> partyStats)
    {
        List<Profile> enemyProfiles = new List<Profile>();

        foreach (PersistanceStats stats in partyStats)
        {
            if (!stats.isDied)
            {
                // Pool'dan Enemy handler alýyoruz
                EnemyProfileLungeHandler lungeHandler = FightPanelObjectPool.instance.GetEnemy();
                Profile spawned = ConfigureProfile(lungeHandler, stats, false);

                if (spawned != null) enemyProfiles.Add(spawned);
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
public enum CharacterType
{
    Profile,
    FireEnemy,
    ShieldEnemy,
    HiveEnemy
}
