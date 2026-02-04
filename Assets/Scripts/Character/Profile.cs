using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class Profile : MonoBehaviour
{
    public static event Action<Profile> OnSomeoneDie;
    public static event Action<string> OnSomeonePlay;




    [HideInInspector] public PersistanceStats stats;
    public ProfileView view;



    public float currentStrength, currentTechnical, currentFocus;

    private float currentSpeed;


    [HideInInspector] public Useable currentSkill;
    [HideInInspector] public Profile currentTarget;

    [HideInInspector] public event Action onHealthChange, onStaminaChange, onManaChange;
    [HideInInspector] public event Action<float> onStrengthChange, onTechnicalChange, onFocusChange, onSpeedChange;




    public bool isDied;
    protected string lastTargetName;

  //battlespawnerda kullanýlabilir
    public void Setup(PersistanceStats persistentData)
    {
        // Dýþarýdaki kalýcý datayý bu profile baðla
        stats = persistentData;
        // Sahneye hazýrla
        gameObject.SetActive(true);

        onHealthChange.Invoke();
        onStaminaChange.Invoke();
        onManaChange.Invoke();


        onStrengthChange.Invoke(currentStrength);
        onTechnicalChange.Invoke(currentTechnical);
        onFocusChange.Invoke(currentFocus);
        onSpeedChange.Invoke(currentSpeed);
    }


    public abstract void LungeStart();
    public abstract void ChooseSkill(Useable skill);
    public  void SetTarget(Profile profile)
    {
        if(profile == null)//Cok hedefli skillerde
        {
            FinishLunge();
            return;
        }
        currentTarget = profile;

        FinishLunge();//!
    }
    public void FinishLunge()
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
        else if (currentTarget && currentTarget.isDied)
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
        onHealthChange?.Invoke();
    }
    public void SetStamina(float amount)
    {
        stats.currentStamina = amount;
        onStaminaChange?.Invoke();
    }
    public void SetMana(float amount)
    {
        stats.currentMana = amount;
        onManaChange?.Invoke();
    }

    public void ForceChangeHealth(float amount)//Overhealth
    {
        stats.currentHealth += amount;
        if (stats.currentHealth <= 0)
        {
            stats.currentHealth = 0;
            Die();
        }
        onHealthChange?.Invoke();
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
        onHealthChange?.Invoke();
    }


    public void ChangeStrength(float amount)
    {
        currentStrength += amount;
        onStrengthChange?.Invoke(currentStrength);
    }
    public void ChangeTechnical(float amount)
    {
        currentTechnical += amount;
        onTechnicalChange?.Invoke(currentTechnical);
    }
    public void ChangeFocus(float amount)
    {
        currentFocus += amount;
        onFocusChange?.Invoke(currentFocus);
    }
    public void ChangeSpeed(float amount)
    {
        currentSpeed += amount;
        onSpeedChange?.Invoke(currentSpeed);
    }


    public void ResetStats()
    {
        isDied = false;
        currentStrength = stats.strength;
        onStrengthChange?.Invoke(currentStrength);
        currentTechnical = stats.technical;
        onTechnicalChange?.Invoke(currentTechnical);
        currentFocus = stats.focus;
        onFocusChange?.Invoke(currentFocus);

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





}
