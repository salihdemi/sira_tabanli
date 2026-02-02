using System;
using UnityEngine;

public abstract class Profile : MonoBehaviour
{
    public static event Action<Profile> OnSomeoneDie;
    public static event Action<string> OnSomeonePlay;




    [HideInInspector] public PersistanceStats stats;
    public void Setup(PersistanceStats persistentData)
    {
        // Dýþarýdaki kalýcý datayý bu profile baðla
        stats = persistentData;

        // Sahneye hazýrla
        gameObject.SetActive(true);
        Debug.Log(name);
        onHealthChange.Invoke(stats.currentHealth);
        onPowerChange. Invoke(currentPower);
        onSpeedChange. Invoke(currentSpeed);
    }

    public ProfileView view;


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
        lastTargetName = currentTarget.name;

        LungeEnd();//!
    }
    public void LungeEnd()
    {
        TurnScheduler.CheckNextCharacter();
    }



    public void Play()
    {
        string text;
        if (isDied)
        {
            text = name + " " + lastTargetName + "'a vurmadý çünkü " + name + " öldü";
        }
        else if (currentTarget.isDied)
        {
            text = name + " " + lastTargetName + "'a vurmadý çünkü " + lastTargetName + " öldü";
        }
        else
        {
            text = name + " " + lastTargetName + "'a " + currentSkill.name + " yaptý";
            currentSkill.Method(this, currentTarget);
        }
        //Debug.Log(text);
        OnSomeonePlay?.Invoke(text);
    }



    public void ClearSkillAndTarget()
    {
        currentTarget = null;
        currentSkill = null;
    }

    



    public void SetHealth(float amount)
    {
        stats.currentHealth = amount;


        onHealthChange?.Invoke(stats.currentHealth);
    }

    public void ForceChangeHealth(float amount)//Overhealth
    {
        stats.currentHealth += amount;
        if (stats.currentHealth <= 0)
        {
            stats.currentHealth = 0;
            Die();
        }
        onHealthChange?.Invoke(stats.currentHealth);
    }
    public void AddToHealth(float amount)
    {
        stats.currentHealth += amount;
        if (stats.currentHealth > stats.maxHealth)
        {
            stats.currentHealth = stats.maxHealth;
        }
        if (stats.currentHealth <= 0)
        {
            Die();
        }
        onHealthChange?.Invoke(stats.currentHealth);
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
        isDied = false;
        currentPower = stats.basePower;
        onPowerChange?.Invoke(currentPower);

        currentSpeed = stats.baseSpeed;
        onSpeedChange?.Invoke(currentSpeed);
    }
    public void Die()
    {
        isDied = true;
        stats.isDied = true;
        TurnScheduler.HandleProfileDeath(this);
        FightManager.SetDefaultTarget();
        OnSomeoneDie.Invoke(this);
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
