using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    public TextMeshProUGUI sceneNoText, sceneDataText, staticDataText;

    public static SaveTest instance;


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        GetTexts();
        GetButtons();
    }

    private void GetButtons()
    {
        Button save = GameObject.Find("Save").GetComponent<Button>();
        save.onClick.RemoveAllListeners();
        save.onClick.AddListener(() => Save(0));

        Button load = GameObject.Find("Load").GetComponent<Button>();
        load.onClick.RemoveAllListeners();
        load.onClick.AddListener(() => Load(0));
    }

    private void GetTexts()
    {
        sceneDataText = GameObject.Find("SahneDatasý").GetComponent<TextMeshProUGUI>();
        staticDataText = GameObject.Find("Statik data").GetComponent<TextMeshProUGUI>();
        sceneNoText = GameObject.Find("SahnenoText").GetComponent<TextMeshProUGUI>();
    }

    private string GetPath(int slotIndex) => Application.persistentDataPath + "/save_" + slotIndex + ".json";
    public void Save(int slotIndex)
    {
        SaveData data = new SaveData();

        GetTexts();
        data.savedScene = SceneManager.GetActiveScene().buildIndex;//sahne numarasý

        data.deadEnemyIDsInScene.Clear();

        data.deadEnemyIDsInScene.Add(sceneDataText.text);//0a sahne datasý
        Debug.Log(data.deadEnemyIDsInScene[0]);
        data.deadEnemyIDsInScene.Add(staticDataText.text);//1e static data




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


        StartCoroutine(LoadAsyncProcess(data));



        //SceneManager.LoadScene(data.savedScene);//sahne numarasý




    }






    private IEnumerator LoadAsyncProcess(SaveData data)
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        if (data.savedScene != SceneManager.GetActiveScene().buildIndex)
        {
            Debug.Log("async basliyor");
            // 2. Sahneyi asenkron olarak yükle
            AsyncOperation operation = SceneManager.LoadSceneAsync(data.savedScene);

            // 3. Sahne tamamen yüklenene kadar burada DUR ve BEKLE
            while (!operation.isDone) yield return null;// Sahne yüklenene kadar her frame bekle

            Debug.Log("Sahne yüklendi.");
            //yield return new WaitForSeconds(1);
        }
        // --- BURAYA GELDÝÐÝNDE ARTIK YENÝ SAHNE %100 YÜKLENDÝ ---

        // 4. Þimdi Find iþlemlerini yapabilirsin, hata almazsýn


        Debug.Log(data.deadEnemyIDsInScene[0]);

        GetTexts();
        sceneDataText.text = data.deadEnemyIDsInScene[0];//0dan sahne datasý

        staticDataText.text = data.deadEnemyIDsInScene[1];//1den static data


    }
}
