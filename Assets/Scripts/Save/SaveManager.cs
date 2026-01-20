using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public PartyManager partyManager;
    public GameObject player;

    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";

    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();
        data.saveDate = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        data.playerX = player.transform.position.x;
        data.playerY = player.transform.position.y;

        // Açýlmýþ müttefikleri isimleriyle kaydet
        foreach (var ally in partyManager.allUnlockedAllys)
        {
            data.savedAllys.Add(new AllySaveData { characterID = ally.originData.name });
        }

        // Aktif partiyi isimleriyle kaydet
        foreach (var p in partyManager.partyStats)
        {
            data.currentPartyIDs.Add(p.originData.name);
        }

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

        // Kütüphaneyi Resources klasöründen otomatik yükle
        CharacterData[] allPossible = Resources.LoadAll<CharacterData>("Characters");

        partyManager.allUnlockedAllys.Clear();
        partyManager.partyStats.Clear();

        foreach (var saved in data.savedAllys)
        {
            // Ýsim eþleþmesine göre orijinal ScriptableObject'i bul
            CharacterData original = System.Array.Find(allPossible, x => x.name == saved.characterID);

            if (original != null)
            {
                PersistanceStats newStats = new PersistanceStats();
                newStats.originData = original;
                partyManager.allUnlockedAllys.Add(newStats);

                // Eðer aktif partideyse listeye ekle ve iþaretle
                if (data.currentPartyIDs.Contains(saved.characterID))
                {
                    newStats.isInParty = true;
                    partyManager.partyStats.Add(newStats);
                }
            }
        }

        player.transform.position = new Vector3(data.playerX, data.playerY, 0);
        Debug.Log($"Slot {slotIndex} yüklendi.");
    }

    public bool HasSave(int slotIndex) => File.Exists(GetPath(slotIndex));
}