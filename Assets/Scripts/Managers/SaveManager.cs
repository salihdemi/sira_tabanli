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

    //public GameObject player;//playerý bulmasý gerek
    public static int saveName;


    private static string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";
    public static bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));





    public static SaveData Save(int slotIndex)
    {
        Debug.Log("Save");
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        saveName++;

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
        SaveDeadEnemiesInScene(data);
    }
    private static void LoadSceneData(SaveData data)
    {
        //player.transform.position = new Vector3(data.playerX, data.playerY, 0);
        LoadEnemiesInScene(data);
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
        SaveScene(data);
    }
    private static void LoadStaticData(SaveData data)
    {
        LoadUnlockedAllies(data);
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





    private static int SkillToInt(_Skill skill)
    {
        int skillIndex = dataBase.skillsDataBase.IndexOf(skill);

        if (skillIndex == -1)
        {
            Debug.LogWarning("Database de olmayan skill eklendi");
            dataBase.skillsDataBase.Add(skill);
            skillIndex = dataBase.skillsDataBase.IndexOf(skill);
        }

        return skillIndex;
    }
    private static _Skill IntToSkill(int listNumber)
    {
        if (dataBase.skillsDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }

        _Skill skill = dataBase.skillsDataBase[listNumber];



        return skill;
    }



    private static AllySaveData PersistanceStatsToAllyData(PersistanceStats ally)
    {
        AllySaveData allySaveData = new AllySaveData();
        allySaveData.name = ally._name;

        allySaveData.currentHealth = ally.currentHealth; // Mevcut caný
        allySaveData.maxHealth = ally.maxHealth;         // Maksimum caný
        allySaveData.basePower = ally.basePower;         // Gücü
        allySaveData.baseSpeed = ally.baseSpeed;         // Hýzý


        allySaveData.isDied = ally.isDied;               // Ölü olup olmadýðý
        allySaveData.isInParty = ally.isInParty;         // Partide olup olmadýðý


        allySaveData.sprite = SpriteToInt(ally.sprite);
        allySaveData.attackSkill = SkillToInt(ally.attack);

        allySaveData.skills.Clear();
        for (int i = 0; i < ally.skills.Count; i++)
        {
            allySaveData.skills.Add(SkillToInt(ally.skills[i]));
        }


        //skilller listesi
        return allySaveData;
    }
    private static PersistanceStats AllyDataToPersistanceStats(AllySaveData allySaveData)
    {
        PersistanceStats persistanceStats = new PersistanceStats();

        persistanceStats._name = allySaveData.name;

        persistanceStats.currentHealth = allySaveData.currentHealth; // Mevcut caný
        persistanceStats.maxHealth = allySaveData.maxHealth;         // Maksimum caný
        persistanceStats.basePower = allySaveData.basePower;         // Gücü
        persistanceStats.baseSpeed = allySaveData.baseSpeed;         // Hýzý


        persistanceStats.isDied = allySaveData.isDied;               // Ölü olup olmadýðý
        persistanceStats.isInParty = allySaveData.isInParty;         // Partide olup olmadýðý


        persistanceStats.sprite = IntToSprite(allySaveData.sprite);
        persistanceStats.attack = IntToSkill(allySaveData.attackSkill);

        persistanceStats.skills.Clear();
        for (int i = 0; i < persistanceStats.skills.Count; i++)
        {
            persistanceStats.skills.Add(IntToSkill(allySaveData.skills[i]));
        }


        return persistanceStats;
    }

    #endregion
}


public class DataVisualizer: MonoBehaviour
{
    public SaveData data;
}