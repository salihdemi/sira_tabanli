using TMPro;
using UnityEngine;

public class ProfileUIManager : MonoBehaviour
{

    [SerializeField] private Profile profile;

    [SerializeField] private TextMeshProUGUI healthText, staminaText, manaText;
    [SerializeField] private TextMeshProUGUI strengthText, technicalText, focusText, speedText;
    private void OnEnable()
    {
        profile.onHealthChange += WriteHealth;
        profile.onStaminaChange += WriteStamina;
        profile.onManaChange += WriteMana;

        profile.onStrengthChange += WriteStrength;
        profile.onTechnicalChange += WriteTechnical;
        profile.onFocusChange += WriteFocus;
        profile.onSpeedChange  += WriteSpeed;
    }
    private void OnDisable()
    {
        profile.onHealthChange -= WriteHealth;
        profile.onStaminaChange -= WriteStamina;
        profile.onManaChange -= WriteMana;

        profile.onStrengthChange -= WriteStrength;
        profile.onTechnicalChange -= WriteTechnical;
        profile.onFocusChange -= WriteFocus;
        profile.onSpeedChange -= WriteSpeed;
    }
    //Barlar
    public void WriteHealth()
    {
        PersistanceStats stats = profile.stats;
        healthText.text = stats.currentHealth + "/" + stats.maxHealth;
    }
    public void WriteStamina()
    {
        PersistanceStats stats = profile.stats;
        staminaText.text = stats.currentStamina + "/" + stats.maxStamina;
    }
    public void WriteMana()
    {
        PersistanceStats stats = profile.stats;
        if (stats.maxMana <= 0)
        {
            manaText.gameObject.SetActive(false);
            return;
        }
        manaText.text = stats.currentMana + "/" + stats.maxMana;
    }


    //Statlar
    public void WriteStrength(float currentStrength)
    {
        strengthText.text = currentStrength.ToString();
    }
    public void WriteTechnical(float currentTechnical)
    {
        technicalText.text = currentTechnical.ToString();
    }
    public void WriteFocus(float currentFocus)
    {
        focusText.text = currentFocus.ToString();
    }
    public void WriteSpeed(float currentSpeed)
    {
        speedText.text = currentSpeed.ToString();
    }
}
