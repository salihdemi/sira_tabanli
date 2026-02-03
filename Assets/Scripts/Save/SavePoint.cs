using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject saveMenuPanel; // Inspector'dan SavePanel'i buraya sürükle
    private bool isPlayerInRange = false;

    [SerializeField] private Vector3 spawnPos;

    [SerializeField] private MainCharacterMoveable character;

    private void Awake()//!
    {
        // 1. Önce sahnedeki ana Canvas'ý bul (Genelde ismi "Canvas" olur)
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // 2. Canvas'ýn altýndaki "SavePanel" isimli objeyi (kapalý olsa bile) bulur
            Transform panelTransform = canvas.transform.Find("SavePanel");

            if (panelTransform != null)
            {
                saveMenuPanel = panelTransform.gameObject;
            }
        }

        if (saveMenuPanel == null)
        {
            Debug.LogError("SavePoint: SavePanel bulamadý! Canvas altýndaki ismi 'SavePanel' mi?");
        }

        if(character == null)
        {
            character = GameObject.FindAnyObjectByType<MainCharacterMoveable>();
            if(character == null)
            {
                Debug.LogError("SavePoint Karakter bulamadý");
            }
        }
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenSaveMenu();
        }
    }

    public void OpenSaveMenu()
    {
        if (saveMenuPanel == null) return;

        SaveManager.currentSavePoint = this;
        saveMenuPanel.SetActive(true);
        // Menü açýldýðýnda karakterin hareketini durdurmak isteyebilirsin
        //Time.timeScale = 0f; // Oyunu duraklat (opsiyonel)

    }

    public void CloseSaveMenu()
    {   
        if (saveMenuPanel == null) return;

        SaveManager.currentSavePoint = null;
        saveMenuPanel.SetActive(false);
        //Time.timeScale = 1f; // Oyunu devam ettir
    }

    public void PlacePlayer()
    {
        if (character != null)
        {
            Transform characterTransform = character.transform;
            characterTransform.position = spawnPos;
        }
    }//bolunebilir

    // Trigger kontrolü
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Kaydetmek için E tuþuna bas.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            CloseSaveMenu();
        }
    }
}