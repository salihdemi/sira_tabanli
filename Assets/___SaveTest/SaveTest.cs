using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTest : MonoBehaviour
{
    public TextMeshProUGUI text;

    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";
    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();


        data.saveDate = text.text;



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



        text.text = data.saveDate;


    }
}
