using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Profile : MonoBehaviour
{
    public ProfileView view;






    public CharacterBase BaseData;


    private float currentHealth;
    private float currentPower;
    private float currentSpeed;


    [HideInInspector] public _Skill currentSkill;
    [HideInInspector] public Profile currentTarget;

    [HideInInspector] public event Action<float> onHealthChange, onPowerChange, onSpeedChange;










    private void Start()
    {
        ChangeHealth(BaseData.maxHealth);
        ResetStats();
    }

    public abstract void LungeStart();
    public abstract void ChooseSkill(_Skill skill);
    public abstract void SetTarget(Profile profile);
    public abstract void LungeEnd();



    public void Play()
    {
        if (this && currentTarget)
        {
            currentSkill.Method(this, currentTarget);
        }
    }



    public void ClearSkillAndTarget()
    {
        currentTarget = null;
        currentSkill = null;
    }

    




    public void ForceChangeHealth(float amount)//Overhealth
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        onHealthChange?.Invoke(currentHealth);
    }
    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > BaseData.maxHealth)
        {
            currentHealth = BaseData.maxHealth;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
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



    public void ResetStats()
    {
        currentPower = BaseData.basePower;
        onPowerChange?.Invoke(currentPower);

        currentSpeed = BaseData.baseSpeed;
        onSpeedChange?.Invoke(currentSpeed);
    }
    public void Die()
    {
        FightManager.instance.HandleProfileDeath(this);
        FightManager.instance.turnScheduler.RemoveFromQueue(this);
        Destroy(gameObject);
    }





    public float GetPower()
    {
        return currentPower;
    }
    public float GetSpeed()
    {
        return currentSpeed;
    }
}
