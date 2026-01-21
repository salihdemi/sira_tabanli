using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

public class SaveManager : MonoBehaviour
{
    public PartyManager partyManager;
    public GameObject player;

    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";

    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        data.playerX = player.transform.position.x; //position yerine kayýt noktasý olacak!
        data.playerY = player.transform.position.y; //position yerine kayýt noktasý olacak!

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

        LoadUnlockedAllies(data);

        player.transform.position = new Vector3(data.playerX, data.playerY, 0); //position yerine kayýt noktasý olacak!
        Debug.Log($"Slot {slotIndex} yüklendi.");
    }


    public bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));




    private void SaveUnlockedAllies(SaveData data)
    {
        // Açýlmýþ müttefikleri isimleriyle kaydet
        foreach (PersistanceStats ally in partyManager.allUnlockedAllies)
        {
            data.savedAllys.Add(new AllySaveData
            {
                //characterID = ally.originData.name, // ScriptableObject dosya adý
                currentHealth = ally.currentHealth, // Mevcut caný
                maxHealth = ally.maxHealth,         // Maksimum caný
                basePower = ally.basePower,         // Gücü
                baseSpeed = ally.baseSpeed,         // Hýzý
                isDied = ally.isDied,               // Ölü olup olmadýðý
                isInParty = ally.isInParty          // Partide olup olmadýðý

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
            newStats.sprite = saved.sprite;

            newStats.maxHealth = saved.maxHealth;
            newStats.currentHealth = saved.currentHealth;
            newStats.basePower = saved.basePower;
            newStats.baseSpeed = saved.baseSpeed;
            newStats.isDied = saved.isDied;
            newStats.isInParty = saved.isInParty;




            partyManager.allUnlockedAllies.Add(newStats);



            if (newStats.isInParty)
            {
                partyManager.TryAddToParty(newStats);
            }

        }
    }
}