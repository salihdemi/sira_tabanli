using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    public string ID;

    [SerializeField] private GameObject saveMenuPanel;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private MainCharacterMoveable character;

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform panelTransform = canvas.transform.Find("SavePanel");
            if (panelTransform != null) saveMenuPanel = panelTransform.gameObject;
        }

        if (saveMenuPanel == null) Debug.LogError("SavePoint: SavePanel bulunamadı!");

        if (character == null)
        {
            character = GameObject.FindAnyObjectByType<MainCharacterMoveable>();
            if (character == null) Debug.LogError("SavePoint: Karakter bulunamadı!");
        }
    }

    public void Interact()
    {
        OpenSaveMenu();
    }

    public void OpenSaveMenu()
    {
        if (saveMenuPanel == null) return;
        SaveManager.currentSavePoint = this;
        saveMenuPanel.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        if (saveMenuPanel == null) return;
        SaveManager.currentSavePoint = null;
        saveMenuPanel.SetActive(false);
    }

    public void PlacePlayer()
    {
        if (character != null)
            character.transform.position = spawnPos;
    }
}
