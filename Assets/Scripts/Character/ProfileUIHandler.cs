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
        profile.onHealthChange -= WriteHealth;
        profile.onHealthChange -= WriteHealth;

        profile.onStrengthChange -= WriteStrength;
        profile.onTechnicalChange -= WriteTechnical;
        profile.onFocusChange -= WriteFocus;

        profile.onSpeedChange -= WriteSpeed;
    }

    //Barlar
    public void WriteHealth(float currentHealth)
    {
        healthText.text = currentHealth.ToString();
    }
    public void WriteStamina(float currentStamina)
    {
        staminaText.text = currentStamina.ToString();
    }
    public void WriteMana(float currentMana)
    {
        manaText.text = currentMana.ToString();
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
