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
            if (SaveManager.instance.HasSave(i))
            {
                canContinue = true;
                break;
            }
        }
        continueButton.interactable = canContinue;
    }
    public void NewGame()
    {
        // 1. Mevcut (varsayýlan) verileri sýfýrla
        // 2. Ýlk sahneye (Intro/Tutorial) geç
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