using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.SceneManagement;

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
    //public GameObject player;//playerý bulmasý gerek




    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";

    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");




        SaveScene(data);
        SaveSceneData(data);
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









        LoadSceneAndData(data);


    }



    public bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));

    private void SaveSceneData(SaveData data)
    {
        //data.playerX = player.transform.position.x; //position yerine kayýt noktasý olacak!
        //data.playerY = player.transform.position.y; //position yerine kayýt noktasý olacak!
        SaveDeadEnemiesInScene(data);
    }
    private void LoadSceneData(SaveData data)
    {
        //player.transform.position = new Vector3(data.playerX, data.playerY, 0);
        LoadEnemiesInScene(data);
    }


    private void SaveDeadEnemiesInScene(SaveData data)
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
    private void LoadEnemiesInScene(SaveData data)
    {
        // Sahnedeki bütün düþman gruplarýný tek tek gez
        foreach (EnemyGroup group in EnemyGroup.GroupsInScene)
        {
            // SORU: Bu grubun ID'si, kaydettiðimiz "Ölüler Listesi"nde var mý?
            Debug.Log(group +"-------");
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


    private void SaveUnlockedAllies(SaveData data)
    {
        // Açýlmýþ müttefikleri isimleriyle kaydet
        PartyManager partyManager = FindAnyObjectByType<PartyManager>();
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

                sprite = SpriteToInt(ally.sprite),

                attackSkill = SkillToInt(ally.attack)
                //skilller listesi

            });

        }
    }
    private void LoadUnlockedAllies(SaveData data)
    {
        PartyManager partyManager = FindAnyObjectByType<PartyManager>();
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



            newStats.sprite = IntToSprite(saved.sprite);

            newStats.attack = IntToSkill(saved.attackSkill);
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
    private void LoadSceneAndData(SaveData data)
    {
        // Coroutine'i baþlatýyoruz
        instance.StartCoroutine(LoadAsyncProcess(data));
    }









    private IEnumerator LoadAsyncProcess(SaveData data)
    {
        if(data.savedScene != SceneManager.loadedSceneCount)
        {
            Debug.Log(SceneManager.loadedSceneCount);
            Debug.Log("async basliyor");
            // 2. Sahneyi asenkron olarak yükle
            AsyncOperation operation = SceneManager.LoadSceneAsync(data.savedScene);

            // 3. Sahne tamamen yüklenene kadar burada DUR ve BEKLE
            while (!operation.isDone) yield return null;// Sahne yüklenene kadar her frame bekle

            Debug.Log("Sahne yüklendi.");
            yield return new WaitForSeconds(1);
        }

        // --- BURAYA GELDÝÐÝNDE ARTIK YENÝ SAHNE %100 YÜKLENDÝ ---
        // 4. Þimdi Find iþlemlerini yapabilirsin, hata almazsýn

        //player = GameObject.FindWithTag("Player");

        LoadUnlockedAllies(data);
        LoadSceneData(data);

    }







    private int SpriteToInt(Sprite sprite)
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
    private Sprite IntToSprite(int listNumber)
    {
        if (dataBase.spritesDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }
        Debug.Log(dataBase.spritesDataBase.Count);
        Sprite sprite = dataBase.spritesDataBase[listNumber];



        return sprite;
    }





    private int SkillToInt(_Skill skill)
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
    private _Skill IntToSkill(int listNumber)
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