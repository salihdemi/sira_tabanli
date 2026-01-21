using UnityEngine;
using TMPro;
using System.IO;

public class SaveSlotUI : MonoBehaviour
{
    public int slotIndex;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI infoText;
    private SaveManager saveManager;

    public void Setup(SaveManager manager)
    {
        saveManager = manager;
        RefreshUI();
    }

    public void RefreshUI()
    {
        string path = Application.persistentDataPath + "/save_" + slotIndex + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            dateText.text = data.saveDate;
        }
        else
        {
            dateText.text = "Boþ Slot";
            infoText.text = "Yeni Oyun";
        }
    }

    // Unity Inspector'da Save butonuna baðla
    public void OnClickSave()
    {
        saveManager.Save(slotIndex);
        RefreshUI();
    }

    // Unity Inspector'da Load butonuna baðla
    public void OnClickLoad()
    {
        saveManager.Load(slotIndex);
    }
}