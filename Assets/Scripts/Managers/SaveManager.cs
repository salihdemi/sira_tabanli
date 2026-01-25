using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler arasý hayatta kal
        }
        else
        {
            Destroy(gameObject); // Eðer baþka bir tane oluþursa onu yok et
        }

        //
    }

    public DataBase dataBase;
    public PartyManager partyManager;
    public GameObject player;




    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";

    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        data.playerX = player.transform.position.x; //position yerine kayýt noktasý olacak!
        data.playerY = player.transform.position.y; //position yerine kayýt noktasý olacak!




        SaveScene(data);

        SaveEnemies(data);
        SaveUnlockedAllies(data);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slotIndex), json);
        Debug.Log($"Slot {slotIndex} kaydedildi: " + GetPath(slotIndex));
    }


    public void Load(int slotIndex)
    {
        string path = GetPath(slotIndex);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);




        LoadScene(data);
        currentData = data;

        SceneManager.sceneLoaded += OnSceneLoad;




        Debug.Log($"Slot {slotIndex} yüklendi.");
    }
    SaveData currentData;
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoad;

        player = GameObject.FindWithTag("Player");
        partyManager = FindAnyObjectByType<PartyManager>();
        Load2(currentData);
    }
    private void Load2(SaveData data)
    {
        LoadEnemies(data);
        LoadUnlockedAllies(data);

        player.transform.position = new Vector3(data.playerX, data.playerY, 0); //position yerine kayýt noktasý olacak!
    }

    public bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));




    private void SaveEnemies(SaveData data)
    {
        // Listeyi temizle
        data.deadEnemyIDs.Clear();

        // Manager'daki TÜM düþmanlarý (ölü/diri) kontrol et
        foreach (EnemyGroup group in EnemyManager.instance.allNormalGroups)
        {
            // Eðer obje kapalýysa (ölüyse), ismini listeye yaz
            if (!group.gameObject.activeSelf)
            {
                data.deadEnemyIDs.Add(group.groupID);
            }
        }
    }




    private void LoadEnemies(SaveData data)
    {
        // Sahnedeki bütün düþman gruplarýný tek tek gez
        foreach (EnemyGroup group in EnemyManager.instance.allNormalGroups)
        {
            // SORU: Bu grubun ID'si, kaydettiðimiz "Ölüler Listesi"nde var mý?
            bool isDead = data.deadEnemyIDs.Contains(group.groupID);

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
    private void SaveUnlockedAllies(SaveData data)
    {
        // Açýlmýþ müttefikleri isimleriyle kaydet
        foreach (PersistanceStats ally in partyManager.allUnlockedAllies)
        {
            data.savedAllys.Add(new AllySaveData
            {
                //characterID = ally.originData.name, // ScriptableObject dosya adý
                name = ally._name,


                currentHealth = ally.currentHealth, // Mevcut caný
                maxHealth = ally.maxHealth,         // Maksimum caný
                basePower = ally.basePower,         // Gücü
                baseSpeed = ally.baseSpeed,         // Hýzý
                isDied = ally.isDied,               // Ölü olup olmadýðý
                isInParty = ally.isInParty,         // Partide olup olmadýðý

                sprite = GetListNumberFromSprite(ally.sprite),

                attackSkill = GetListNumberFromSkill(ally.attack)
                //skilller listesi

            });

        }
    }
    private void LoadUnlockedAllies(SaveData data)
    {
        partyManager.allUnlockedAllies.Clear();
        partyManager.partyStats.Clear();

        foreach (AllySaveData saved in data.savedAllys)
        {

            PersistanceStats newStats = new PersistanceStats();

            newStats._name = saved.name;

            newStats.maxHealth = saved.maxHealth;
            newStats.currentHealth = saved.currentHealth;
            newStats.basePower = saved.basePower;
            newStats.baseSpeed = saved.baseSpeed;
            newStats.isDied = saved.isDied;
            newStats.isInParty = saved.isInParty;



            newStats.sprite = GetSpriteFromListNumber(saved.sprite);

            newStats.attack = GetSkillFromListNumber(saved.attackSkill);
            //skilller listesi


            partyManager.allUnlockedAllies.Add(newStats);



            if (newStats.isInParty)
            {
                partyManager.TryAddToParty(newStats);
            }

        }
    }

    private void SaveScene(SaveData data)
    {
        data.savedScene = SceneManager.loadedSceneCount;
    }
    private void LoadScene(SaveData data)
    {
        SceneManager.LoadScene(data.savedScene);
    }









    private int GetListNumberFromSprite(Sprite sprite)
    {
        int spriteIndex = dataBase.spritesDataBase.IndexOf(sprite);

        if (spriteIndex == -1)
        {
            Debug.LogWarning("Database de olmayan skill eklendi");
            dataBase.spritesDataBase.Add(sprite);
            spriteIndex = dataBase.spritesDataBase.IndexOf(sprite);
        }

        return spriteIndex;
    }
    private Sprite GetSpriteFromListNumber(int listNumber)
    {
        if (dataBase.spritesDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }

        Sprite sprite = dataBase.spritesDataBase[listNumber];



        return sprite;
    }





    private int GetListNumberFromSkill(_Skill skill)
    {
        int skillIndex = dataBase.skillsDataBase.IndexOf(skill);

        if (skillIndex == -1)
        {
            Debug.LogError("Database de olmayan skill");
            dataBase.skillsDataBase.Add(skill);
            skillIndex = dataBase.skillsDataBase.IndexOf(skill);
        }

        return skillIndex;
    }
    private _Skill GetSkillFromListNumber(int listNumber)
    {
        if (dataBase.skillsDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }

        _Skill skill = dataBase.skillsDataBase[listNumber];



        return skill;
    }
}