using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionPanel : MonoBehaviour
{
    public static CharacterActionPanel instance;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private Transform panelsParent;

    [SerializeField] private Button attackButton;
    [SerializeField] private Button skillsButton;
    [SerializeField] private Button foodsButton;
    [SerializeField] private Button toysButton;

    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject foodsPanel;
    [SerializeField] private GameObject toysPanel;

    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Birden fazla CharacterActionPanel var");
            Debug.Log(name);
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        CheckInstance();
    }



    public void DisableAllPanels()
    {
        skillsPanel.SetActive(false);
        foodsPanel.SetActive(false);
        toysPanel.SetActive(false);
    }

    public void WriteThings(AllyProfile character)
    {
        WriteName(character);
        WriteAttack(character);
        WriteSkillsPanel(character);
        WriteFoodsPanel();
        WriteToysPanel();
    }


    #region Write
    private void WriteName(AllyProfile character)
    {
        nameText.text = character.name;
    }
    private void WriteAttack(AllyProfile profile)
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => profile.ChooseSkill(profile.BaseData.attack));
    }
    private void WriteSkillsPanel(AllyProfile profile)
    {
        foreach (Transform child in skillsPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }

        foreach (_Skill skill in profile.BaseData.skills)
        {
            //skill butonu ekle
        }
    }
    private void WriteFoodsPanel()
    {
        //Yemekleri yaz, !bu fonksiyona hiç gerek olmayadabilir
    }
    private void WriteToysPanel()
    {
        //Oyuncaklarý yaz, !bu fonksiyona hiç gerek olmayadabilir
    }
    #endregion
}
