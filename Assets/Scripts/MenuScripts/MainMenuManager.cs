using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject saveSlotsPanel; // Eðer slot seçtiriceksen


    public Button continueButton;
    void Start()
    {
        // SaveManager'da daha önce yazdýðýmýz HasSave fonksiyonunu kullan
        // Eðer en az 1 slotta kayýt varsa tuþu aktif et
        bool canContinue = false;
        for (int i = 0; i < 3; i++) // 3 slotun olduðunu varsayalým
        {
            if (SaveManager.HasSave(i))
            {
                canContinue = true;
                break;
            }
        }
        if(continueButton != null)
            continueButton.interactable = canContinue;
    }

    [SerializeField] private CharacterData[] partyDatas;
    public void NewGame()
    {
        // 1. Mevcut (varsayýlan) verileri sýfýrla
        // 2. Ýlk sahneye (Intro/Tutorial) geç

        for (int i = 0; i < partyDatas.Length; i++)
        {
            PartyManager.UnlockAlly(partyDatas[i]);
        }
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        saveSlotsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}