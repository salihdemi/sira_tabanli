using TMPro;
using UnityEngine;

public class ProfileUIManager : MonoBehaviour
{

    [SerializeField] private Profile profile;

    [SerializeField] private TextMeshProUGUI healthText, powerText, speedText;

    private void OnEnable()
    {
        profile.onHealthChange += WriteHealth;
        profile.onPowerChange  += WritePower;
        profile.onSpeedChange  += WriteSpeed;
    }
    private void OnDisable()
    {
        profile.onHealthChange -= WriteHealth;
        profile.onPowerChange -= WritePower;
        profile.onSpeedChange -= WriteSpeed;
    }
    public void WriteHealth(float currentHealth)
    {
        healthText.text = currentHealth.ToString();
    }
    public void WritePower(float currentPower)
    {
        powerText.text = currentPower.ToString();
    }
    public void WriteSpeed(float currentSpeed)
    {
        speedText.text = currentSpeed.ToString();
    }
}
