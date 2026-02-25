
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










    public static event Action<Profile> OnSomeoneDie;




    [HideInInspector] public PersistanceStats stats;
    public ProfileButtonHandler view;//kaldýrýlabilirse iyi olur
    public ProfileLungeHandler lungeHandler;//kaldýrýlabilirse iyi olur



    public float currentStrength, currentTechnical, currentFocus, currentSpeed;



    [HideInInspector] public event Action onHealthChange, onStaminaChange, onManaChange;
    [HideInInspector] public event Action<float> onStrengthChange, onTechnicalChange, onFocusChange, onSpeedChange;

    
    private void OnConsumeMana(float amount)
    {
        foreach (EnemyProfileLungeHandler enemy in TurnScheduler.ActiveEnemyProfiles)
        {
            if (enemy.profile.stats?.talimsan is ManaLeech_Talisman talisman)
            {
                talisman.AbsorbMana(enemy.profile, this, amount);
            }
        }
    }//yeri ve parametreleri deðiþtirilmesi gerekebilir?!


    public bool isDied;
    public string lastTargetName;



    //private?
    public int hitCountForTour;
    public int mute;//aktif degil
    public int fire;
    public bool taunt;
    public bool willTaunt;







    #region Setup
    public void Setup(PersistanceStats persistanceStats)
    {
        gameObject.SetActive(true);

        stats = persistanceStats;
        gameObject.name = persistanceStats._name + " profile";

        GetComponent<Image>().sprite = persistanceStats.sprite;


        ResetStatus(persistanceStats);
        ResetStats();


        TurnScheduler.onTourEnd += BurnProcess;
        TurnScheduler.onTourEnd += StatusProcess;//event birikmesi olur mu??!!
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

    #endregion

    /*
    #region LungeSequence
    public abstract void LungeStart();
    public abstract void ChooseSkill(Skill skill);
    public void SetTarget(Profile profile)
    {
        if (profile == null)//Cok hedefli skillerde
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
        TurnScheduler.CheckNextCharacterToLunge();
    }

    #endregion
    
    #region PlaySequence
    public bool Play()
    {
        if (isDied) return false;

        bool needTarget = currentSkill.targetType == TargetType.enemy || currentSkill.targetType == TargetType.ally;
        bool targetValid = !needTarget || (currentTarget != null && !currentTarget.isDied);

        if (targetValid)
        {
            TurnScheduler.AddAction(currentSkill.Method(this, currentTarget));
            return true; // Baþarýyla sýraya eklendi
        }

        return false; // Oynayamadý
    }

    #endregion
    */


    #region Status

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

    public void AddToHealth(float amount, Profile dealer)
    {
        //kalkaný varsa
        if (stats.currentShields.Count > 0 && amount < 0) DamageShield(-amount);
        else
        {
            stats.currentHealth += amount;

            //0-max arasýna sabitle
            stats.currentHealth = Mathf.Clamp(stats.currentHealth, 0, stats.maxHealth);

            onHealthChange?.Invoke();

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

    }
    public void AddShield(float amount)
    {
        stats.currentShields.Add(amount);

        onHealthChange?.Invoke(); // ondamageShield???
        return;
    }
    private void DamageShield(float damage)
    {
        stats.currentShields[0] -= damage;

        if (stats.currentShields[0] <= 0)
        {
            stats.currentShields.RemoveAt(0); // En üstteki kalkaný yok et
            //Debug.Log($"{name} kalkaný kýrýldý! Kalan hasar emildi.");

        }
        else
        {
            //Debug.Log($"{name} kalkaný darbe aldý. Kalan kalkan caný: {stats.currentShields[0]}");
        }

        onHealthChange?.Invoke(); // ondamageShield???
        return;
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
            if(this is Profile)
            {
                OnConsumeMana(-amount);//belki burda caðýrýlmamalý!?
            }
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
        willTaunt = true;
    }
    private void TauntProcess()
    {
        if (taunt) LeaveTaunt();

        else if (willTaunt) EnterTaunt();
    }

    private void EnterTaunt()
    {
        if (this is Profile ally) FightManager.tauntedAlly = ally;//ally-enemy kontrolü!!!!!!!!!!!!1
        else if (this is Profile enemy) FightManager.tauntedEnemy = enemy;//
        willTaunt = false;
        taunt = true;
    }
    private void LeaveTaunt()
    {
        if (this is Profile) FightManager.tauntedAlly = null;
        else if (this is Profile) FightManager.tauntedEnemy = null;
        taunt = false;
    }

    private void Die()
    {
        stats.talimsan?.OnDie(this, null, 0);//sýra yanlýþ olabilir

        isDied = true;
        stats.isDied = true;
        TurnScheduler.HandleProfileDeath(this);
        FightManager.SetDefaultTarget();

        //CombatManager.AddAction(Method(Died);//ölüm mesajý!!

        OnSomeoneDie.Invoke(this);
    }

    #endregion



    public bool IsEnoughForSkill(CharacterSkill skill)
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
    private void BurnProcess()
    {
        fire--;
        if (fire > 0) AddToHealth(-5, null);
    }
    private void StatusProcess()
    {
        if (mute > 0) mute--;
        BurnProcess();
        TauntProcess();
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












    /*
    private static string GetActionText(Profile profile)
    {
        if (profile.isDied) return $"{profile.name} öldüðü için hamle yapamadý.";
        if (profile.mute > 0) return $"{profile.name} susturulduðu için büyü yapamadý.";
        if (profile.currentTarget != null && profile.currentTarget.isDied) return $"{profile.name}, hedefi öldüðü için vuruþunu boþa salladý.";

        return $"{profile.name}, {profile.lastTargetName} hedefine {profile.currentUseable.name} kullandý.";
    }*/
}
