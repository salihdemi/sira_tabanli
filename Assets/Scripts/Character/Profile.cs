using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Profile : MonoBehaviour
{
    public static event Action<Profile> OnSomeoneDie;
    public static event Action OnSomeoneLungeEnd;





    [HideInInspector] public PersistanceStats stats;
    public void Setup(PersistanceStats persistentData)
    {
        // Dýþarýdaki kalýcý datayý bu profile baðla
        stats = persistentData;

        // Sahneye hazýrla
        gameObject.SetActive(true);
        onHealthChange.Invoke(currentHealth);
        onPowerChange. Invoke(currentPower);
        onSpeedChange. Invoke(currentSpeed);
    }

    public ProfileView view;

    private float currentHealth;
    private float currentPower;
    private float currentSpeed;


    [HideInInspector] public _Skill currentSkill;
    [HideInInspector] public Profile currentTarget;

    [HideInInspector] public event Action<float> onHealthChange, onPowerChange, onSpeedChange;




    public bool isDied;
    private string lastTargetName; 





    public abstract void LungeStart();
    public abstract void ChooseSkill(_Skill skill);
    public  void SetTarget(Profile profile)
    {
        if(profile == null)//!
        {
            LungeEnd();//!
            return;
        }
        currentTarget = profile;
        Debug.Log(currentTarget);
        lastTargetName = currentTarget.name;

        LungeEnd();//!
    }
    public void LungeEnd()
    {
        OnSomeoneLungeEnd?.Invoke();
    }



    public void Play()
    {
        if (isDied)
        {
            Debug.Log(name + " " + lastTargetName + "'a vurmadý çünkü " + name + " öldü");
        }
        else if (currentTarget.isDied)
        {
            Debug.Log(name + " " + lastTargetName + "'a vurmadý çünkü " + lastTargetName + " öldü");
        }
        else
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
        if (currentHealth > stats.maxHealth)
        {
            currentHealth = stats.maxHealth;
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
        currentPower = stats.basePower;
        onPowerChange?.Invoke(currentPower);

        currentSpeed = stats.baseSpeed;
        onSpeedChange?.Invoke(currentSpeed);
    }
    public void Die()
    {
        isDied = true;
        OnSomeoneDie?.Invoke(this);
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
