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
        //data.playerX = player.transform.position.x; //position yerine kayýt noktasý olacak!
        //data.playerY = player.transform.position.y; //position yerine kayýt noktasý olacak!
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
        data.savePointName = currentSavePoint._name;
    }
    private static void LoadSavePoint(SaveData data)
    {
        if (!string.IsNullOrEmpty(data.savePointName)) // SaveData içindeki deðiþkenin adý
        {
            // 1. Sahnedeki bütün SavePoint scriptlerini bul
            SavePoint[] allSavePoints = Object.FindObjectsByType<SavePoint>(FindObjectsSortMode.None);
            SavePoint targetPoint = null;

            // 2. Ýçlerinden _name deðiþkeni bizimkiyle eþleþeni seç
            foreach (SavePoint sp in allSavePoints)
            {
                if (sp._name == data.savePointName)
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
                Debug.LogError($"ID'si '{data.savePointName}' olan SavePoint sahnede bulunamadý!");
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
        SaveScene(data);
        SaveUnlockedAllies(data);
        SaveConsumables(data);
    }
    private static void LoadStaticData(SaveData data)
    {
        LoadUnlockedAllies(data);
        LoadConsumables(data);
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


    private static void SaveConsumables(SaveData data)
    {
        data.consumableNumbers.Clear();
        data.consumableAmounts.Clear();
        foreach (var pair in InventoryManager.consumables)
        {
            data.consumableNumbers.Add(UseableToInt(pair.Key)); // SO'nun adýný kaydet
            data.consumableAmounts.Add(pair.Value);  // Adedini kaydet
        }

    }
    private static void LoadConsumables(SaveData data)
    {
        InventoryManager.consumables.Clear();
        // Yemekleri Yükle
        for (int i = 0; i < data.consumableNumbers.Count; i++)
        {
            // Resources/Foods/ klasörü altýndaki SO'yu isminden bul
            //Food foodSO = Resources.Load<Food>("Foods/" + data.foodNames[i]);

            Consumable foodSO = (Consumable)IntToUseable(data.consumableNumbers[i]);

            if (foodSO != null)
            {
                InventoryManager.consumables.Add(foodSO, data.consumableAmounts[i]);
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
            Debug.LogError("Liste dýþýnda");
            return null;
        }
        Sprite sprite = dataBase.spritesDataBase[listNumber];



        return sprite;
    }





    private static int UseableToInt(Useable useable)
    {
        int useableIndex = dataBase.useablesDataBase.IndexOf(useable);

        if (useableIndex == -1)
        {
            Debug.LogWarning("Database de olmayan skill eklendi");
            dataBase.useablesDataBase.Add(useable);
            useableIndex = dataBase.useablesDataBase.IndexOf(useable);
        }

        return useableIndex;
    }
    private static Useable IntToUseable(int listNumber)
    {
        if (dataBase.useablesDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }

        Useable useable = dataBase.useablesDataBase[listNumber];



        return useable;
    }


    private static int WeaponToInt(Weapon weapon)
    {
        int weaponIndex = dataBase.weaponsDataBase.IndexOf(weapon);

        if (weaponIndex == -1)
        {
            Debug.LogWarning("Database de olmayan sprite eklendi");
            dataBase.weaponsDataBase.Add(weapon);
            weaponIndex = dataBase.weaponsDataBase.IndexOf(weapon);
        }

        return weaponIndex;
    }
    private static Weapon IntToWeapon(int listNumber)
    {
        if (dataBase.weaponsDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }
        Weapon weapon = dataBase.weaponsDataBase[listNumber];



        return weapon;
    }


    private static int ItemToInt(Item item)
    {
        int weaponIndex = dataBase.itemsDataBase.IndexOf(item);

        if (weaponIndex == -1)
        {
            Debug.LogWarning("Database de olmayan sprite eklendi");
            dataBase.itemsDataBase.Add(item);
            weaponIndex = dataBase.itemsDataBase.IndexOf(item);
        }

        return weaponIndex;
    }
    private static Item IntToItem(int listNumber)
    {
        if (dataBase.itemsDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }
        Item item = dataBase.itemsDataBase[listNumber];



        return item;
    }


    private static int CharmToInt(Talisman charm)
    {
        int weaponIndex = dataBase.charmsDataBase.IndexOf(charm);

        if (weaponIndex == -1)
        {
            Debug.LogWarning("Database de olmayan sprite eklendi");
            dataBase.charmsDataBase.Add(charm);
            weaponIndex = dataBase.charmsDataBase.IndexOf(charm);
        }

        return weaponIndex;
    }
    private static Talisman IntToCharm(int listNumber)
    {
        if (dataBase.charmsDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }
        Talisman charm = dataBase.charmsDataBase[listNumber];



        return charm;
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


        allySaveData.baseSpeed = ally.baseSpeed;         // Hýzý


        allySaveData.isDied = ally.isDied;               // Ölü olup olmadýðý
        allySaveData.isInParty = ally.isInParty;         // Partide olup olmadýðý


        allySaveData.weapon = WeaponToInt(ally.weapon);
        allySaveData.item = ItemToInt(ally.item);
        allySaveData.charm = CharmToInt(ally.talimsan);

        allySaveData.sprite = SpriteToInt(ally.sprite);
        allySaveData.attackSkill = UseableToInt(ally.attack);

        allySaveData.skills.Clear();
        for (int i = 0; i < ally.skills.Count; i++)
        {
            allySaveData.skills.Add(UseableToInt(ally.skills[i]));
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
        persistanceStats.baseSpeed = allySaveData.baseSpeed;         // Hýz



        persistanceStats.weapon = IntToWeapon(allySaveData.weapon);
        persistanceStats.item = IntToItem(allySaveData.item);
        persistanceStats.talimsan = IntToCharm(allySaveData.charm);

        //skiller
        persistanceStats.attack = (Skill)IntToUseable(allySaveData.attackSkill);//attack

        persistanceStats.skills.Clear();
        for (int i = 0; i < allySaveData.skills.Count; i++)          //Skiller listesi
            persistanceStats.skills.Add((Skill)IntToUseable(allySaveData.skills[i]));




        //diðer
        persistanceStats.sprite = IntToSprite(allySaveData.sprite);  //görsel
        persistanceStats.isDied = allySaveData.isDied;               // Ölüm durumu
        persistanceStats.isInParty = allySaveData.isInParty;         // Partide aktiflik durumu

        return persistanceStats;
    }

    #endregion
}