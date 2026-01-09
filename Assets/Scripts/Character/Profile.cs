using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Profile : MonoBehaviour
{
    public Button button;//!
    public CharacterBase character;


    private float currentHealth;
    private float currentPower;
    private float currentSpeed;


    [HideInInspector] public Action<Profile, Profile> Lunge;
    [HideInInspector] public Profile Target;//fonksiyon içi deðiþken olabilir belki

    [HideInInspector] public Action<float> onHealthChange, onPowerChange, onSpeedChange;







    private void Start()
    {
        ChangeHealth(character.maxHealth);
        ResetStats();
    }

    public abstract void Play();
    public abstract void Over();
    public void ResetStats()
    {
        currentPower = character.basePower;
        onPowerChange?.Invoke(currentPower);

        currentSpeed = character.baseSpeed;
        onSpeedChange?.Invoke(currentSpeed);
    }
    public abstract void SetLunge(_Skill skill);
    public abstract void OpenPickTargetMenu(_Skill skill);
    public void ClearLungeAndTarget()
    {
        Lunge = null;
        Target = null;
    }

    public void ForceChangeHealth(float amount)//Overhealth
    {
        currentHealth += amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            //öl


            //herkes öldü mü diye kontrol et
        }
        onHealthChange?.Invoke(currentHealth);
    }
    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > character.maxHealth)
        {
            currentHealth = character.maxHealth;
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        onHealthChange?.Invoke(currentHealth);
    }
    public void ChangePower(float amount)
    {
        currentPower += amount;
        onPowerChange?.Invoke(currentPower);
    }
    public void ChangeSpeed(float amount)
    {
        currentSpeed += amount;
        onSpeedChange?.Invoke(currentSpeed);
    }


    public bool IsDied()
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }//!



    public float GetPower()
    {
        return currentPower;
    }
    public float GetSpeed()
    {
        return currentSpeed;
    }
}
