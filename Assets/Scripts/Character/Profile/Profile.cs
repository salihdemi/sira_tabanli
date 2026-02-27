
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Profile : MonoBehaviour
{
    [HideInInspector] public PersistanceStats stats;
    [HideInInspector] public ProfileLungeHandler lungeHandler;//kaldýrýlabilirse iyi olurdu

    public bool isAlly;

    #region Events
    [HideInInspector] public event Action onHealthChange, onStaminaChange, onManaChange;
    [HideInInspector] public event Action<float> onStrengthChange, onTechnicalChange, onFocusChange, onSpeedChange;
    public static event Action<Profile> OnSomeoneDie;
    protected virtual void NotifyHealthChanged()
    {
        onHealthChange?.Invoke();
    }
    #endregion

    #region Stats
    public float currentStrength, currentTechnical, currentFocus, currentSpeed;
    #endregion
    #region Effects
    private int hitCountForTour;
    private int mute;//aktif degil
    protected int fire;
    private bool taunt;
    private bool willTaunt;
    #endregion




    #region Setup
    
    public void Setup(PersistanceStats persistanceStats)
    {
        gameObject.SetActive(true);

        stats = persistanceStats;
        gameObject.name = persistanceStats._name + " profile";

        GetComponent<Image>().sprite = persistanceStats.sprite;


        ResetStatus(persistanceStats);
        ResetStats();


        TurnScheduler.onTourEnd += StatusProcess;
        TurnScheduler.onTourEnd += ResetHitCount;
        //Talisman?

    }
    private void ResetStatus(PersistanceStats persistanceStats)
    {
        SetHealth(persistanceStats.currentHealth);
        SetStamina(persistanceStats.currentStamina);
        SetMana(persistanceStats.currentMana);
    }
    public void ResetStats()
    {
        currentStrength = stats.strength;
        onStrengthChange?.Invoke(currentStrength);

        currentTechnical = stats.technical;
        onTechnicalChange?.Invoke(currentTechnical);

        currentFocus = stats.focus;
        onFocusChange?.Invoke(currentFocus);

        currentSpeed = stats.speed;
        onSpeedChange?.Invoke(currentSpeed);
    }

    public void UnSetup()
    {
        TurnScheduler.onTourEnd -= StatusProcess;
        TurnScheduler.onTourEnd -= ResetHitCount;
        //Talisman?
    }
    private void OnDestroy()
    {
        UnSetup();
    }
    #endregion




    #region SetValue

    //setler olustururken calismali sadece
    public void SetHealth(float amount)
    {
        stats.currentHealth = amount;
        NotifyHealthChanged();
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
    #endregion
    #region StatusChange
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
    public virtual void AddToHealth(float amount, Profile dealer)
    {
        stats.currentHealth += amount;

        //0-max arasýna sabitle
        stats.currentHealth = Mathf.Clamp(stats.currentHealth, 0, stats.maxHealth);

        NotifyHealthChanged();

        if (amount < 0)
        {
            float damage = -amount;

            hitCountForTour++;

            if (stats.currentHealth <= 0)
            {
                Die();
            }

            if (stats.talimsan && dealer)
            {
                stats?.talimsan.OnTakeDamage(this, dealer, damage);
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
            //mana azaltan herhangi birþey sebeð olabilir!
            if(isAlly)
            {
                FightManager.OnAllyConsumeMana(this, -amount);//belki burda caðýrýlmamalý!?
            }
        }
    }

    private void Die()
    {
        stats.talimsan?.OnDie(this, null, 0);//sýra yanlýþ olabilir
        UnSetup();
        stats.isDied = true;
        FightManager.HandleProfileDeath(this);
        FightManager.SetDefaultTarget();

        TurnScheduler.AddAction(DeadMessage());//ölüm mesajý!!

        OnSomeoneDie.Invoke(this);
    }

    #endregion
    #region StatsChange
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
    #endregion



    #region EffectsChange
    public void Taunt()
    {
        willTaunt = true;
    }
    private void TauntProcess()
    {
        if (taunt) LeaveTaunt();

        else if (willTaunt) EnterTaunt();
    }
    private void EnterTaunt()
    {
        if (isAlly) FightManager.tauntedAlly = this;
        else FightManager.tauntedEnemy = this;
        willTaunt = false;
        taunt = true;
    }
    private void LeaveTaunt()
    {
        if (this is Profile) FightManager.tauntedAlly = null;
        else if (this is Profile) FightManager.tauntedEnemy = null;
        taunt = false;
    }



    public void Burn(int amount)
    {
        fire += amount;
    }
    private void BurnProcess()
    {
        fire--;
        if (fire > 0) AddToHealth(-5, null);
    }

    public void Mute(int amount)
    {
        mute += amount;
    }

    private void StatusProcess()
    {
        if (mute > 0) mute--;
        BurnProcess();
        TauntProcess();
    }
    #endregion





    private void ResetHitCount()
    {
        hitCountForTour = 0;
    }



    public bool IsEnoughForSkill(Skill skill)
    {
        bool healthEnough = stats.currentHealth >= skill.healthCost;
        bool staminaEnough = stats.currentStamina >= skill.staminaCost;
        bool manaEnough = stats.currentMana >= skill.manaCost;
        return healthEnough && staminaEnough && manaEnough;
    }







    private void Parryy(Profile owner, Profile target)
    {
        string log = owner.name + " ";
        ConsolePanel.instance.WriteConsole(log);
        TurnScheduler.AddAction(Parry(owner, target));
    }//!
    private IEnumerator Parry(Profile owner, Profile target)
    {

        target.AddToHealth(5, null);

        yield return new WaitForSeconds(1);
    }//!











    private IEnumerator DeadMessage()
    {
        string text = $"{stats._name} öldü";
        ConsolePanel.instance.WriteConsole(text);
        yield return new WaitForSeconds(1f);
    }

    /*private static string GetActionText(Profile profile)
    {
        if (profile.isDied) return $"{profile.name} öldüðü için hamle yapamadý.";
        if (profile.mute > 0) return $"{profile.name} susturulduðu için büyü yapamadý.";
        if (profile.currentTarget != null && profile.currentTarget.isDied) return $"{profile.name}, hedefi öldüðü için vuruþunu boþa salladý.";

        return $"{profile.name}, {profile.lastTargetName} hedefine {profile.currentUseable.name} kullandý.";
    }*/
}

