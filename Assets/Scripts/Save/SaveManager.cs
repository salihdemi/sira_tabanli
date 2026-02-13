using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public static class SaveManager
{
    
    private static DataBase _dataBase; // Bu sadece iç depo
    public static DataBase dataBase // Doðru eriþim noktasý
    {
        get
        {
            if (_dataBase == null)
            {
                _dataBase = Resources.Load<DataBase>("GameDatabase");
                if (_dataBase == null) Debug.LogError("DÝKKAT: Resources klasöründe 'GameDatabase' bulunamadý!");
            }
            return _dataBase;
        }
    }
    
    //public GameObject player;//playerý bulmasý gerek!

    public static SavePoint currentSavePoint;


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
    }
    private static void LoadSceneData(SaveData data)
    {
        LoadSavePoint(data);
        LoadEnemiesInScene(data);
    }

    private static void SaveSavePoint(SaveData data)
    {
        data.savePointID = currentSavePoint.ID;
    }
    private static void LoadSavePoint(SaveData data)
    {
        if (!string.IsNullOrEmpty(data.savePointID)) // SaveData içindeki deðiþkenin adý
        {
            // 1. Sahnedeki bütün SavePoint scriptlerini bul
            SavePoint[] allSavePoints = Object.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);
            SavePoint targetPoint = null;

            // 2. Ýçlerinden _name deðiþkeni bizimkiyle eþleþeni seç
            foreach (SavePoint sp in allSavePoints)
            {
                if (sp.ID == data.savePointID)
                {
                    targetPoint = sp;
                    break;
                }
            }

            // 3. Eðer bulduysak yerleþtir
            if (targetPoint != null)
            {
                targetPoint.PlacePlayer();
                currentSavePoint = targetPoint;
            }
            else
            {
                Debug.LogError($"ID'si '{data.savePointID}' olan SavePoint sahnede bulunamadý!");
            }
        }
    }


    private static void SaveDeadEnemiesInScene(SaveData data)
    {
        // Listeyi temizle
        data.deadEnemyIDsInScene.Clear();

        if (EnemyGroup.GroupsInScene.Count <= 0) return;

        foreach (EnemyGroup group in EnemyGroup.GroupsInScene)
        {
            // Eðer obje kapalýysa (ölüyse), ismini listeye yaz
            if (!group.gameObject.activeSelf)
            {
                data.deadEnemyIDsInScene.Add(group.groupID);
            }
        }
    }
    private static void LoadEnemiesInScene(SaveData data)
    {
        // Sahnedeki bütün düþman gruplarýný tek tek gez
        foreach (EnemyGroup group in EnemyGroup.GroupsInScene)
        {
            // SORU: Bu grubun ID'si, kaydettiðimiz "Ölüler Listesi"nde var mý?
            bool isDead = data.deadEnemyIDsInScene.Contains(group.groupID);

            if (isDead)
            {
                // Listede adý var, demek ki ölmüþ. Kapat.
                group.gameObject.SetActive(false);
            }
            else
            {
                // Listede adý yok, demek ki yaþýyor.
                // Aç ve resetle (Canýný fulle, yerine ýþýnla)
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



        // 1. Bir eylem (Action) tanýmlýyoruz
        UnityAction<Scene, LoadSceneMode> handler = null;

        // 2. Bu eylemin içine ne yapacaðýný yazýyoruz
        handler = (scene, mode) =>
        {
            LoadSceneData(data);
            SceneManager.sceneLoaded -= handler; // Kendi aboneliðini iptal eder (KRÝTÝK)
        };

        // 3. Sahne yükleme event'ine bu eylemi baðlýyoruz
        SceneManager.sceneLoaded += handler;

        // 4. Sahneyi yüklüyoruz
        SceneManager.LoadScene(data.savedScene);
    }


    private static void SaveUnlockedAllies(SaveData data)
    {
        data.savedAllys.Clear();
        // Açýlmýþ müttefikleri isimleriyle kaydet
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



    #endregion
















    #region Convert fonksiyonlari

    private static int SpriteToInt(Sprite sprite)
    {
        int spriteIndex = dataBase.spritesDataBase.IndexOf(sprite);

        if (spriteIndex == -1)
        {
            Debug.LogWarning("Database de olmayan sprite eklendi");
            dataBase.spritesDataBase.Add(sprite);
            spriteIndex = dataBase.spritesDataBase.IndexOf(sprite);
        }

        return spriteIndex;
    }
    private static Sprite IntToSprite(int listNumber)
    {
        if (dataBase.spritesDataBase.Count < listNumber)
        {
            Debug.LogWarning("Liste dýþýnda");
            return null;
        }
        Sprite sprite = dataBase.spritesDataBase[listNumber];



        return sprite;
    }
    
    private static AllySaveData PersistanceStatsToAllyData(PersistanceStats ally)
    {
        AllySaveData allySaveData = new AllySaveData();
        allySaveData.name = ally._name;
        allySaveData.weaponType = ally.weaponType.ToString();

        allySaveData.currentHealth = ally.currentHealth;   // Mevcut caný
        allySaveData.currentStamina = ally.currentStamina; // Mevcut staminasý
        allySaveData.currentMana = ally.currentMana;       // Mevcut manasý

        allySaveData.maxHealth = ally.maxHealth;         // Maksimum caný
        allySaveData.maxStamina = ally.maxStamina;         // Maksimum staminasý
        allySaveData.maxMana = ally.maxMana;         // Maksimum manasý


        allySaveData.strength = ally.strength;      // Gücü
        allySaveData.technical = ally.technical;    // tekniði
        allySaveData.focus = ally.focus;            // focusu


        allySaveData.baseSpeed = ally.speed;         // Hýzý


        allySaveData.isDied = ally.isDied;               // Ölü olup olmadýðý
        allySaveData.isInParty = ally.isInParty;         // Partide olup olmadýðý


        if (ally.weapon) allySaveData.weapon = ally.weapon.name;
        if (ally.item) allySaveData.item = ally.item.name;
        if (ally.talimsan) allySaveData.talisman = ally.talimsan.name;

        allySaveData.sprite = SpriteToInt(ally.sprite);
        allySaveData.attackSkill = ally.attack.name;

        allySaveData.skills.Clear();
        for (int i = 0; i < ally.skills.Count; i++)
        {
            allySaveData.skills.Add(ally.skills[i].name);
        }


        //skilller listesi
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
        persistanceStats.speed = allySaveData.baseSpeed;         // Hýz



        persistanceStats.weapon = FindSOByName<Weapon>(allySaveData.weapon);
        persistanceStats.item = FindSOByName<Item>(allySaveData.item);
        persistanceStats.talimsan = FindSOByName<Talisman>(allySaveData.talisman);

        //skiller
        persistanceStats.attack = FindSOByName<Skill>(allySaveData.attackSkill);//attack

        persistanceStats.skills.Clear();
        for (int i = 0; i < allySaveData.skills.Count; i++)          //Skiller listesi
            persistanceStats.skills.Add(FindSOByName<Skill>(allySaveData.skills[i]));




        //diðer
        persistanceStats.sprite = IntToSprite(allySaveData.sprite);  //görsel
        persistanceStats.isDied = allySaveData.isDied;               // Ölüm durumu
        persistanceStats.isInParty = allySaveData.isInParty;         // Partide aktiflik durumu

        return persistanceStats;
    }

    #endregion





    // --- SAVE FONKSÝYONU ---
    public static void SaveInventoryData(SaveData data)
    {
        InventorySaveData inventoryData = new InventorySaveData();

        // 1. Consumables Paketle
        foreach (var pair in InventoryManager.consumables)
        {
            inventoryData.consumableNames.Add(pair.Key.name);
            inventoryData.consumableAmounts.Add(pair.Value);
        }

        // 2. Silahlarý Paketle
        foreach (var w in InventoryManager.ownedWeapons) inventoryData.ownedWeaponNames.Add(w.name);
        foreach (var w in InventoryManager.equippedWeapons) inventoryData.equippedWeaponNames.Add(w.name);

        // 3. Itemleri Paketle (Hem sahip olunanlar hem takýlý olanlar)
        foreach (var t in InventoryManager.ownedItems) inventoryData.ownedItemNames.Add(t.name);
        foreach (var t in InventoryManager.equippedItems) inventoryData.equippedItemNames.Add(t.name);

        // 4. Talismanlarý Paketle (Hem sahip olunanlar hem takýlý olanlar)
        foreach (var t in InventoryManager.ownedTalismas) inventoryData.ownedTalismanNames.Add(t.name);
        foreach (var t in InventoryManager.equippedTalismans) inventoryData.equippedTalismanNames.Add(t.name);




        data.inventorySaveData = inventoryData;
    }

    // --- LOAD FONKSÝYONU ---
    public static void LoadInventoryData(SaveData data)
    {
        InventorySaveData inventoryData = data.inventorySaveData;

        // 1. Consumables Geri Yükle
        InventoryManager.consumables.Clear();
        for (int i = 0; i < inventoryData.consumableNames.Count; i++)
        {
            // Þimdilik null check ile geçiyoruz, Resources kýsmýnda baðlayacaðýz
            var so = FindSOByName<Consumable>(inventoryData.consumableNames[i]);
            if (so != null) InventoryManager.consumables.Add(so, inventoryData.consumableAmounts[i]);
        }

        // 2. Silahlarý Geri Yükle
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

        // 3. Itemleri Geri Yükle
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

        // 4. Talismanlarý Geri Yükle
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




    private static T FindSOByName<T>(string name) where T : ScriptableObject
    {
        if (string.IsNullOrEmpty(name)) return null;

        string folderPath = "";

        // Tip kontrolü yaparak doðru klasöre yönlendiriyoruz
        if (typeof(T) == typeof(Talisman)) folderPath = "Equipables/Talismans/";
        else if (typeof(T) == typeof(Weapon)) folderPath = "Equipables/Weapons/";
        else if (typeof(T) == typeof(Item)) folderPath = "Equipables/Items/";
        else if (typeof(T) == typeof(Skill)) folderPath = "Skills/";
        else if (typeof(T) == typeof(Consumable)) folderPath = "Consumables/";
        else if (typeof(T) == typeof(Sprite)) folderPath = "Sprites/";

        // Resources.Load, belirtilen klasördeki ismi arar
        T foundSO = Resources.Load<T>(folderPath + name);

        if (foundSO == null)
        {
            Debug.LogError($"HATA: '{name}' isimli {typeof(T).Name} dosyasý 'Resources/{folderPath}' içinde bulunamadý!");
        }

        return foundSO;
    }
}