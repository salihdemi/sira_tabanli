using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public PartyManager partyManager;
    public GameObject player;


    public List<EnemyGroup> enemyGroups;


    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";

    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        Debug.Log(player);
        Debug.Log(player.transform.position);
        Debug.Log(player.transform.position.x);
        data.playerX = player.transform.position.x; //position yerine kayýt noktasý olacak!
        data.playerY = player.transform.position.y; //position yerine kayýt noktasý olacak!


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

        LoadEnemies(data);
        LoadUnlockedAllies(data);

        player.transform.position = new Vector3(data.playerX, data.playerY, 0); //position yerine kayýt noktasý olacak!
        Debug.Log($"Slot {slotIndex} yüklendi.");
    }


    public bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));




    private void SaveEnemies(SaveData data)
    {
        enemyGroups = EnemyManager.instance.allNormalGroups;
        data.savedEnemyGroups.Clear();
        for (int i = 0; i < enemyGroups.Count; i++)
        {
            data.savedEnemyGroups.Add(enemyGroups[i].gameObject.activeSelf);
        }
    }
    private void LoadEnemies(SaveData data)
    {
        for (int i = 0; i < enemyGroups.Count; i++)
        {
            EnemyGroup enemy = enemyGroups[i];


            enemy.gameObject.SetActive(data.savedEnemyGroups[i]);

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

            Debug.Log(saved.name);
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











    private int GetListNumberFromSprite(Sprite sprite)
    {
        int spriteIndex = DataBase.instance.spritesDataBase.IndexOf(sprite);

        if (spriteIndex == -1)
        {
            Debug.LogError("Database de olmayan skill");
            DataBase.instance.spritesDataBase.Add(sprite);
            spriteIndex = DataBase.instance.spritesDataBase.IndexOf(sprite);
        }

        return spriteIndex;
    }
    private Sprite GetSpriteFromListNumber(int listNumber)
    {
        if (DataBase.instance.spritesDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }

        Sprite sprite = DataBase.instance.spritesDataBase[listNumber];



        return sprite;
    }





    private int GetListNumberFromSkill(_Skill skill)
    {
        int skillIndex = DataBase.instance.skillsDataBase.IndexOf(skill);

        if(skillIndex == -1)
        {
            Debug.LogError("Database de olmayan skill");
            DataBase.instance.skillsDataBase.Add(skill);
            skillIndex = DataBase.instance.skillsDataBase.IndexOf(skill);
        }

        return skillIndex;
    }
    private _Skill GetSkillFromListNumber(int listNumber)
    {
        if (DataBase.instance.skillsDataBase.Count < listNumber)
        {
            Debug.LogError("Liste dýþýnda");
            return null;
        }

        _Skill skill = DataBase.instance.skillsDataBase[listNumber];



        return skill;
    }
}