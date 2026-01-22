using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject saveMenuPanel; // Inspector'dan SavePanel'i buraya sürükle
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenSaveMenu();
        }
    }

    public void OpenSaveMenu()
    {
        saveMenuPanel.SetActive(true);
        // Menü açýldýðýnda karakterin hareketini durdurmak isteyebilirsin
        //Time.timeScale = 0f; // Oyunu duraklat (opsiyonel)

    }

    public void CloseSaveMenu()
    {
        saveMenuPanel.SetActive(false);
        //Time.timeScale = 1f; // Oyunu devam ettir
    }



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