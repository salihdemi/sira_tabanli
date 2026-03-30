using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public static class SaveManager
{
    
    private static DataBase _dataBase; // Bu sadece i� depo
    public static DataBase dataBase // Do�ru eri�im noktas�
    {
        get
        {
            if (_dataBase == null)
            {
                _dataBase = Resources.Load<DataBase>("GameDatabase");
                if (_dataBase == null) Debug.LogError("D�KKAT: Resources klas�r�nde 'GameDatabase' bulunamad�!");
            }
            return _dataBase;
        }
    }
    
    //public GameObject player;//player� bulmas� gerek!

    public static SavePoint currentSavePoint;
    private static HashSet<string> _collectedIDs = new HashSet<string>();

    public static void RegisterCollected(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        _collectedIDs.Add(id);
    }


    private static string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";
    public static bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));





    public static SaveData Save(int slotIndex)
    {
        Debug.Log("Save");
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");



        SaveSceneData(data);
        SaveStaticData(data);




        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slotIndex), json);
        Debug.Log($"Slot {slotIndex} kaydedildi: " + GetPath(slotIndex));

        return data;
    }
    public static void Load(int slotIndex)
    {
        Debug.Log("Load");
        string path = GetPath(slotIndex);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);




        LoadStaticData(data);

        if (data.savedScene == SceneManager.GetActiveScene().buildIndex)
        {
            LoadSceneData(data);
        }
        else
        {
            LoadSceneAndData(data);
        }







    }

















    #region SceneData
    private static void SaveSceneData(SaveData data)
    {
        SaveScene(data);
        SaveSavePoint(data);
        SaveDeadEnemiesInScene(data);
        SaveCollectibles(data);
    }
    private static void LoadSceneData(SaveData data)
    {
        LoadSavePoint(data);
        LoadEnemiesInScene(data);
        LoadCollectibles(data);
    }

    private static void SaveSavePoint(SaveData data)
    {
        data.savePointID = currentSavePoint.ID;
    }
    private static void LoadSavePoint(SaveData data)
    {
        if (!string.IsNullOrEmpty(data.savePointID)) // SaveData i�indeki de�i�kenin ad�
        {
            // 1. Sahnedeki b�t�n SavePoint scriptlerini bul
            SavePoint[] allSavePoints = Object.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);
            SavePoint targetPoint = null;

            // 2. ��lerinden _name de�i�keni bizimkiyle e�le�eni se�
            foreach (SavePoint sp in allSavePoints)
            {
                if (sp.ID == data.savePointID)
                {
                    targetPoint = sp;
                    break;
                }
            }

            // 3. E�er bulduysak yerle�tir
            if (targetPoint != null)
            {
                targetPoint.PlacePlayer();
                currentSavePoint = targetPoint;
            }
            else
            {
                Debug.LogError($"ID'si '{data.savePointID}' olan SavePoint sahnede bulunamad�!");
            }
        }
    }


    private static void SaveCollectibles(SaveData data)
    {
        data.collectedIDs.Clear();
        foreach (string id in _collectedIDs)
            data.collectedIDs.Add(id);
    }
    private static void LoadCollectibles(SaveData data)
    {
        _collectedIDs = new HashSet<string>(data.collectedIDs);

        Collectible[] allCollectibles = Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None);
        foreach (Collectible c in allCollectibles)
        {
            if (_collectedIDs.Contains(c.ID))
                Object.Destroy(c.gameObject);
        }
    }

    private static void SaveDeadEnemiesInScene(SaveData data)
    {
        // Listeyi temizle
        data.deadEnemyIDsInScene.Clear();

        if (EnemyGroup.GroupsInScene.Count <= 0) return;

        foreach (EnemyGroup group in EnemyGroup.GroupsInScene)
        {
            // E�er obje kapal�ysa (�l�yse), ismini listeye yaz
            if (!group.gameObject.activeSelf)
            {
                data.deadEnemyIDsInScene.Add(group.groupID);
            }
        }
    }
    private static void LoadEnemiesInScene(SaveData data)
    {
        // Sahnedeki b�t�n d��man gruplar�n� tek tek gez
        foreach (EnemyGroup group in EnemyGroup.GroupsInScene)
        {
            // SORU: Bu grubun ID'si, kaydetti�imiz "�l�ler Listesi"nde var m�?
            bool isDead = data.deadEnemyIDsInScene.Contains(group.groupID);

            if (isDead)
            {
                // Listede ad� var, demek ki �lm��. Kapat.
                group.gameObject.SetActive(false);
            }
            else
            {
                // Listede ad� yok, demek ki ya��yor.
                // A� ve resetle (Can�n� fulle, yerine ���nla)
                group.gameObject.SetActive(true);
                group.ResetGroup();
            }
        }
    }


    #endregion




    #region StaticData
    private static void SaveStaticData(SaveData data)
    {
        SaveUnlockedAllies(data);
        SaveInventoryData(data);
    }
    private static void LoadStaticData(SaveData data)
    {
        LoadUnlockedAllies(data);
        LoadInventoryData(data);
    }


    private static void SaveScene(SaveData data)
    {
        data.savedScene = SceneManager.GetActiveScene().buildIndex;
    }
    private static void LoadSceneAndData(SaveData data)
    {



        // 1. Bir eylem (Action) tan�ml�yoruz
        UnityAction<Scene, LoadSceneMode> handler = null;

        // 2. Bu eylemin i�ine ne yapaca��n� yaz�yoruz
        handler = (scene, mode) =>
        {
            LoadSceneData(data);
            SceneManager.sceneLoaded -= handler; // Kendi aboneli�ini iptal eder (KR�T�K)
        };

        // 3. Sahne y�kleme event'ine bu eylemi ba�l�yoruz
        SceneManager.sceneLoaded += handler;

        // 4. Sahneyi y�kl�yoruz
        SceneManager.LoadScene(data.savedScene);
    }


    private static void SaveUnlockedAllies(SaveData data)
    {
        data.savedAllys.Clear();
        // A��lm�� m�ttefikleri isimleriyle kaydet
        foreach (PersistanceStats ally in PartyManager.allUnlockedAllies)
        {
            data.savedAllys.Add(PersistanceStatsToAllyData(ally));
        }
    }
    private static void LoadUnlockedAllies(SaveData data)
    {
        PartyManager.allUnlockedAllies.Clear();
        PartyManager.partyStats.Clear();

        foreach (AllySaveData saved in data.savedAllys)
        {

            PersistanceStats newStats = AllyDataToPersistanceStats(saved);


            PartyManager.allUnlockedAllies.Add(newStats);


            if (newStats.isInParty)
            {
                PartyManager.TryAddToParty(newStats);
            }

        }
    }


    public static void SaveInventoryData(SaveData data)
    {
        InventorySaveData inventoryData = new InventorySaveData();

        // 1. Consumables Paketle
        foreach (var pair in InventoryManager.consumables)
        {
            inventoryData.consumableNames.Add(pair.Key.name);
            inventoryData.consumableAmounts.Add(pair.Value);
        }

        // 2. Silahlar� Paketle
        foreach (Weapon w in InventoryManager.ownedWeapons) inventoryData.ownedWeaponNames.Add(w.name);
        foreach (Weapon w in InventoryManager.equippedWeapons) inventoryData.equippedWeaponNames.Add(w.name);

        // 3. Itemleri Paketle (Hem sahip olunanlar hem tak�l� olanlar)
        foreach (Item i in InventoryManager.ownedItems) inventoryData.ownedItemNames.Add(i.name);
        foreach (Item i in InventoryManager.equippedItems) inventoryData.equippedItemNames.Add(i.name);

        // 4. Talismanlar� Paketle (Hem sahip olunanlar hem tak�l� olanlar)
        foreach (Talisman t in InventoryManager.ownedTalismas) inventoryData.ownedTalismanNames.Add(t.name);
        foreach (Talisman t in InventoryManager.equippedTalismans) inventoryData.equippedTalismanNames.Add(t.name);




        data.inventorySaveData = inventoryData;
    }
    public static void LoadInventoryData(SaveData data)
    {
        InventorySaveData inventoryData = data.inventorySaveData;

        // 1. Consumables Geri Y�kle
        InventoryManager.consumables.Clear();
        for (int i = 0; i < inventoryData.consumableNames.Count; i++)
        {
            // �imdilik null check ile ge�iyoruz, Resources k�sm�nda ba�layaca��z
            var so = FindSOByName<Consumable>(inventoryData.consumableNames[i]);
            if (so != null) InventoryManager.consumables.Add(so, inventoryData.consumableAmounts[i]);
        }

        // 2. Silahlar� Geri Y�kle
        InventoryManager.ownedWeapons.Clear();
        InventoryManager.equippedWeapons.Clear();
        foreach (string name in inventoryData.ownedWeaponNames)
        {
            Weapon so = FindSOByName<Weapon>(name);
            if (so != null) InventoryManager.ownedWeapons.Add(so);
        }
        foreach (string name in inventoryData.equippedWeaponNames)
        {
            Weapon so = FindSOByName<Weapon>(name);
            if (so != null) InventoryManager.equippedWeapons.Add(so);
        }

        // 3. Itemleri Geri Y�kle
        InventoryManager.ownedItems.Clear();
        InventoryManager.equippedItems.Clear();
        foreach (string name in inventoryData.ownedItemNames)
        {
            Item so = FindSOByName<Item>(name);
            if (so != null) InventoryManager.ownedItems.Add(so);
        }
        foreach (string name in inventoryData.equippedItemNames)
        {
            Item so = FindSOByName<Item>(name);
            if (so != null) InventoryManager.equippedItems.Add(so);
        }

        // 4. Talismanlar� Geri Y�kle
        InventoryManager.ownedTalismas.Clear();
        InventoryManager.equippedTalismans.Clear();
        foreach (string name in inventoryData.ownedTalismanNames)
        {
            Talisman so = FindSOByName<Talisman>(name);
            if (so != null) InventoryManager.ownedTalismas.Add(so);
        }
        foreach (string name in inventoryData.equippedTalismanNames)
        {
            Talisman so = FindSOByName<Talisman>(name);
            if (so != null) InventoryManager.equippedTalismans.Add(so);
        }
    }



    #endregion
















    #region Convert fonksiyonlari
    private static AllySaveData PersistanceStatsToAllyData(PersistanceStats ally)
    {
        AllySaveData allySaveData = new AllySaveData();
        allySaveData.name = ally._name;
        allySaveData.weaponType = ally.weaponType.ToString();

        allySaveData.currentHealth = ally.currentHealth;   // Mevcut can�
        allySaveData.currentStamina = ally.currentStamina; // Mevcut staminas�
        allySaveData.currentMana = ally.currentMana;       // Mevcut manas�

        allySaveData.maxHealth = ally.maxHealth;         // Maksimum can�
        allySaveData.maxStamina = ally.maxStamina;         // Maksimum staminas�
        allySaveData.maxMana = ally.maxMana;         // Maksimum manas�


        allySaveData.strength = ally.strength;      // G�c�
        allySaveData.technical = ally.technical;    // tekni�i
        allySaveData.focus = ally.focus;            // focusu


        allySaveData.baseSpeed = ally.speed;         // H�z�


        allySaveData.isDied = ally.isDied;               // �l� olup olmad���
        allySaveData.isInParty = ally.isInParty;         // Partide olup olmad���


        if (ally.weapon) allySaveData.weapon = ally.weapon.name;
        if (ally.item) allySaveData.item = ally.item.name;
        if (ally.talimsan) allySaveData.talisman = ally.talimsan.name;

        if (ally.sprite != null)
        {
            allySaveData.sprite = ally.sprite.name;
            Debug.Log(ally.sprite.name);
        }

        allySaveData.attackSkill = ally.attack.name;

        allySaveData.unlockedSkills.Clear();
        for (int i = 0; i < ally.unlockedSkills.Count; i++)
        {
            allySaveData.unlockedSkills.Add(ally.unlockedSkills[i].name);
        }

        allySaveData.currentSkills.Clear();
        for (int i = 0; i < ally.currentSkills.Count; i++)
        {
            allySaveData.currentSkills.Add(ally.currentSkills[i].name);
        }


        return allySaveData;
    }
    private static PersistanceStats AllyDataToPersistanceStats(AllySaveData allySaveData)
    {
        PersistanceStats persistanceStats = new PersistanceStats();

        persistanceStats._name = allySaveData.name;                 // isim
        persistanceStats.weaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), allySaveData.weaponType);//test edilmedi

        //can
        persistanceStats.currentHealth = allySaveData.currentHealth; // Mevcut can
        persistanceStats.maxHealth = allySaveData.maxHealth;         // Maksimum can

        //stamina
        persistanceStats.currentStamina = allySaveData.currentStamina;// Mevcut stamina
        persistanceStats.currentStamina = allySaveData.maxStamina;    // Maksimum stamina

        //mana
        persistanceStats.currentMana = allySaveData.currentMana;     // Mevcut mana
        persistanceStats.maxMana = allySaveData.maxMana;             // Maksimum mana



        //statlar
        persistanceStats.strength = allySaveData.strength;           // strength
        persistanceStats.technical = allySaveData.technical;         // technical
        persistanceStats.focus = allySaveData.focus;                 // focus
        persistanceStats.speed = allySaveData.baseSpeed;         // H�z



        persistanceStats.weapon = FindSOByName<Weapon>(allySaveData.weapon);
        persistanceStats.item = FindSOByName<Item>(allySaveData.item);
        persistanceStats.talimsan = FindSOByName<Talisman>(allySaveData.talisman);

        //skiller
        persistanceStats.attack = FindSOByName<Attack>(allySaveData.attackSkill);//attack

        persistanceStats.unlockedSkills.Clear();
        for (int i = 0; i < allySaveData.unlockedSkills.Count; i++)          //Skiller listesi
            persistanceStats.unlockedSkills.Add(FindSOByName<Skill>(allySaveData.unlockedSkills[i]));

        persistanceStats.currentSkills.Clear();
        for (int i = 0; i < allySaveData.currentSkills.Count; i++)          //Skiller listesi
            persistanceStats.currentSkills.Add(FindSOByName<Skill>(allySaveData.currentSkills[i]));



        //di�er
        //persistanceStats.sprite = IntToSprite(allySaveData.sprite);  //g�rsel

        persistanceStats.sprite = FindSpriteByName(allySaveData.sprite);


        persistanceStats.isDied = allySaveData.isDied;               // �l�m durumu
        persistanceStats.isInParty = allySaveData.isInParty;         // Partide aktiflik durumu

        return persistanceStats;
    }

    #endregion





    private static T FindSOByName<T>(string name) where T : ScriptableObject
    {
        if (string.IsNullOrEmpty(name)) return null;

        string folderPath = "";

        // Tip kontrol� yaparak do�ru klas�re y�nlendiriyoruz
        if (typeof(T) == typeof(Talisman)) folderPath = "Equipables/Talismans/";
        else if (typeof(T) == typeof(Weapon)) folderPath = "Equipables/Weapons/";
        else if (typeof(T) == typeof(Item)) folderPath = "Equipables/Items/";
        else if (typeof(T) == typeof(Skill)) folderPath = "Skills/";
        else if (typeof(T) == typeof(Attack)) folderPath = "Attacks/";
        else if (typeof(T) == typeof(Consumable)) folderPath = "Consumables/";
        else if (typeof(T) == typeof(Sprite)) folderPath = "Sprites/";

        // Resources.Load, belirtilen klas�rdeki ismi arar
        T foundSO = Resources.Load<T>(folderPath + name);

        if (foundSO == null)
        {
            Debug.LogError($"HATA: '{name}' isimli {typeof(T).Name} dosyas� 'Resources/{folderPath}' i�inde bulunamad�!");
        }

        return foundSO;
    }
    private static Sprite FindSpriteByName(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;

        // Sprite'lar �zel bir klas�rde oldu�u i�in yolu direkt veriyoruz
        Sprite foundSprite = Resources.Load<Sprite>("Sprites/" + name);

        if (foundSprite == null)
        {
            Debug.LogError($"HATA: '{name}' isimli Sprite 'Resources/Sprites/' i�inde bulunamad�!");
        }

        return foundSprite;
    }
}