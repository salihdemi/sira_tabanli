using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionPanel : MonoBehaviour
{

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


    private void Awake()
    {
        AllyProfile.OnAnyAllyLungeStart += OpenWriteThings;
        AllyProfile.OnAnyAllyChoseSkill += OnSkillSelectedWrapper;
    }
    private void OnDestroy()
    {
        AllyProfile.OnAnyAllyLungeStart -= OpenWriteThings;
        AllyProfile.OnAnyAllyChoseSkill -= OnSkillSelectedWrapper;
    }



    public void OpenWriteThings(AllyProfile character)
    {
        gameObject.SetActive(true);

        WriteName(character);
        WriteAttack(character);
        WriteSkillsPanel(character);
        WriteFoodsPanel();
        WriteToysPanel();
    }

    public void CloseAndDisableAllPanels()
    {
        skillsPanel.SetActive(false);
        foodsPanel.SetActive(false);
        toysPanel.SetActive(false);

        gameObject.SetActive(false);
    }

    private void OnSkillSelectedWrapper(Profile character, _Skill skill)
    {
        // Event fýrladýðýnda buraya gelecek, 
        // biz de asýl kapatma fonksiyonunu tetikleyeceðiz.
        CloseAndDisableAllPanels();
    }

    #region Write
    private void WriteName(AllyProfile character)
    {
        nameText.text = character.name;
    }
    private void WriteAttack(AllyProfile profile)
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(() => profile.ChooseSkill(profile.stats.attack));
    }
    private void WriteSkillsPanel(AllyProfile profile)
    {
        foreach (Transform child in skillsPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);//pool tipi yap!
        }

        foreach (_Skill skill in profile.stats.skills)
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
