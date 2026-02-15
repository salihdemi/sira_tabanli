using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public abstract class Profile : MonoBehaviour
{
    public static event Action<Profile> OnSomeoneDie;
    public static event Action<string> OnSomeonePlay;




    [HideInInspector] public PersistanceStats stats;
    public ProfileView view;



    public float currentStrength, currentTechnical, currentFocus, currentSpeed;


    [HideInInspector] public Useable currentUseable;
    [HideInInspector] public Profile currentTarget;

    [HideInInspector] public event Action onHealthChange, onStaminaChange, onManaChange;
    [HideInInspector] public event Action<float> onStrengthChange, onTechnicalChange, onFocusChange, onSpeedChange;




    public bool isDied;
    protected string lastTargetName;



    //private
    public int hitCountForTour;
    public int mute;
    public int fire;
    public bool taunt;








    //battlespawnerda kullanýlabilir

    public void Setup(PersistanceStats persistanceStats)
    {
        gameObject.SetActive(true);

        stats = persistanceStats;
        gameObject.name = persistanceStats._name + " profile";

        GetComponent<Image>().sprite = persistanceStats.sprite;


        ResetStatus(persistanceStats);
        ResetStats();


        TurnScheduler.onTourEnd += DamageIfBurning;
        TurnScheduler.onTourEnd += DecreaseStatus;
        TurnScheduler.onTourEnd += ResetHitCount;
        //Talisman

    }

    private void ResetStatus(PersistanceStats persistanceStats)
    {
        SetHealth(persistanceStats.currentHealth);
        SetStamina(persistanceStats.currentStamina);
        SetMana(persistanceStats.currentMana);
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
        lastTargetName = currentTarget.name;

        FinishLunge();//!
    }
    public void FinishLunge()
    {
        TurnScheduler.CheckNextCharacter();
    }



    public void PlayIfAlive()
    {
        string text;
        if (isDied)
        {
            lastTargetName = 
            text = name + " " + lastTargetName + "'a vurmadý çünkü " + name + " öldü";
        }
        else if (currentTarget && currentTarget.isDied)
        {
            text = name + " " + lastTargetName + "'a vurmadý çünkü " + lastTargetName + " öldü";
        }
        else if (mute > 0)
        {
            text = name + " susturuldu";
        }
        else
        {
            text = name + " " + lastTargetName + "'a " + currentUseable.name + " yaptý";
            Play();
        }
        //Debug.Log(text);
        OnSomeonePlay?.Invoke(text);//WriteConsole
    }

    private void Play()
    {
        Debug.Log($"Kullanýlan Skill: {currentUseable._name} | Mana Cost: {currentUseable.manaCost}");
        AddToHealth(-currentUseable.healthCost, this);
        AddToMana(-currentUseable.manaCost);
        AddToStamina(-currentUseable.staminaCost);
        currentUseable.Method(this, currentTarget);
    }

    public void ClearSkillAndTarget()
    {
        currentTarget = null;
        currentUseable = null;
    }

    


    //setler olustururken calismali sadece
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

    /*public void ForceChangeHealth(float amount)//Overhealth
    {
        stats.currentHealth += amount;
        if (stats.currentHealth <= 0)
        {
            stats.currentHealth = 0;
            Die();
        }
        onHealthChange?.Invoke();
    }*/
    public void AddToHealth(float amount, Profile owner)
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
        if (amount < 0)
        {
            hitCountForTour++;
            if (stats.talimsan && owner)
            {
                stats?.talimsan.OnTakeDamage(owner, -amount);
            }
        }
    }
    public void AddToStamina(float amount)
    {
        stats.currentStamina += amount;
        if (stats.currentStamina > stats.maxStamina)
        {
            stats.currentStamina = stats.maxStamina;
        }
        if (stats.currentStamina < 0)
        {
            Debug.LogWarning(stats._name + " stamina 0'ýn altýna düþtü");
        }
        onStaminaChange?.Invoke();
    }
    public void AddToMana(float amount)
    {
        stats.currentMana += amount;
        if (stats.currentMana > stats.maxMana)
        {
            stats.currentMana = stats.maxMana;
        }
        if (stats.currentMana < 0)
        {
            Debug.LogWarning(stats._name + " mana 0'ýn altýna düþtü");
        }
        onManaChange?.Invoke();
        if (amount < 0)
        {
            OnConsumeMana(-amount);
        }
    }


    public void AddToStrength(float amount)
    {
        currentStrength += amount;
        onStrengthChange?.Invoke(currentStrength);
    }
    public void AddToTechnical(float amount)
    {
        currentTechnical += amount;
        onTechnicalChange?.Invoke(currentTechnical);
    }
    public void AddToFocus(float amount)
    {
        currentFocus += amount;
        onFocusChange?.Invoke(currentFocus);
    }
    public void AddToSpeed(float amount)
    {
        currentSpeed += amount;
        onSpeedChange?.Invoke(currentSpeed);
    }


    public void Taunt()
    {
        if (this is AllyProfile ally)
        {
            FightManager.tauntedAlly = ally;
        }
        else if (this is EnemyProfile enemy)
        {
            FightManager.tauntedEnemy = enemy;
        }
        taunt = true;
    }
    public void FinishTaunt()
    {
        if (this is AllyProfile) FightManager.tauntedAlly = null;
        else if (this is EnemyProfile) FightManager.tauntedEnemy = null;
        taunt = false;
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

        currentSpeed = stats.speed;
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





    public bool IsEnoughForSkill(Useable skill)
    {
        bool healthEnough = stats.currentHealth >= skill.healthCost;
        bool staminaEnough = stats.currentStamina >= skill.staminaCost;
        bool manaEnough = stats.currentMana >= skill.manaCost;
        return healthEnough && staminaEnough && manaEnough;
    }


    private void ResetHitCount()
    {
        hitCountForTour = 0;
    }
    private void DamageIfBurning()
    {
        if (fire > 0) AddToHealth(-5, null);
    }
    private void DecreaseStatus()
    {
        if (mute > 0) mute--;
        if (fire > 0) fire--;



        FinishTaunt();
    }

    private void OnConsumeMana(float amount)
    {
        List<Profile> etherics = new List<Profile>();//Eterik tür
        int ethericCount = 0;

        foreach (Profile enemy in TurnScheduler.ActiveEnemyProfiles)
        {
            if (enemy != null)//eterikse!!!
            {
                etherics.Add(enemy);
                ethericCount++;
            }
        }
        foreach (Profile etheric in etherics)//profile yerine eterik!!!
        {
            etheric.AddToMana(amount / ethericCount);
        }
    }



}
