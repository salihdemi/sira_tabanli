using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Profile : MonoBehaviour
{
    public Button button;//!
    public CharacterBase BaseData;


    private float currentHealth;
    private float currentPower;
    private float currentSpeed;


    [HideInInspector] public Action<Profile, Profile> Lunge;
    [HideInInspector] public Profile Target;

    [HideInInspector] public event Action<float> onHealthChange, onPowerChange, onSpeedChange;



    [HideInInspector] public event Action<Profile> onProfileDie;


    [HideInInspector] public event Action onTurnStarted, onTurnEnded;



    public void OnProfileButtonPressed()
    {
        TargetingSystem.instance.OnProfileClicked(this);
    }

    public void SetSelectable(bool state, Profile currentCaster)
    {
        button.interactable = state;
        button.onClick.AddListener(() => currentCaster.SetTarget(this));
        // Ýstersen burada seçilebilir olanlarýn etrafýnda parlama efekti açabilirsin
    }



    private void Start()
    {
        ChangeHealth(BaseData.maxHealth);
        ResetStats();
    }

    public virtual void TurnStart()
    {
        onTurnStarted?.Invoke();
    }
    public virtual void TurnEnd()
    {
        onTurnEnded?.Invoke();
    }
    public void ResetStats()
    {
        currentPower = BaseData.basePower;
        onPowerChange?.Invoke(currentPower);

        currentSpeed = BaseData.baseSpeed;
        onSpeedChange?.Invoke(currentSpeed);
    }
    public abstract void SetLunge(_Skill skill);
    public void SetTarget(Profile profile)
    {
        Target = profile;

        TurnEnd();//!
    }
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
            Die();


            //herkes öldü mü diye kontrol et
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



    public void Die()
    {
        //Listelerden sil
        onProfileDie?.Invoke(this);
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
