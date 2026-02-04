using TMPro;
using UnityEngine;

public class ProfileUIManager : MonoBehaviour
{

    [SerializeField] private Profile profile;

    [SerializeField] private TextMeshProUGUI healthText, strengthText, technicalText, focusText, speedText;

    private void OnEnable()
    {
        profile.onHealthChange += WriteHealth;

        profile.onStrengthChange += WriteStrength;
        profile.onTechnicalChange += WriteTechnical;
        profile.onFocusChange += WriteFocus;

        profile.onSpeedChange  += WriteSpeed;
    }
    private void OnDisable()
    {
        profile.onHealthChange -= WriteHealth;

        profile.onStrengthChange -= WriteStrength;
        profile.onTechnicalChange -= WriteTechnical;
        profile.onFocusChange -= WriteFocus;

        profile.onSpeedChange -= WriteSpeed;
    }
    public void WriteHealth(float currentHealth)
    {
        healthText.text = currentHealth.ToString();
    }


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
