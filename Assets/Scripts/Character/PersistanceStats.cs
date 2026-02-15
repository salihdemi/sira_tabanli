using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class PersistanceStats
{
    [Header("Bio")]
    public string _name;
    public WeaponType weaponType;

    [Header("Status")]
    public float currentHealth;
    public float currentStamina;
    public float currentMana;

    [Header("MaxStats")]
    public float maxHealth;
    public float maxStamina;
    public float maxMana;
    [Header("Stats")]
    public float strength;
    public float technical;
    public float focus;
    public float speed;

    [Header("Stuff")]
    public Weapon weapon;
    public Item item;
    public Talisman talimsan;


    [Header("Sprite")]
    public Sprite sprite;

    [Header("Skills")]
    public Attack attack;
    public List<Skill> currentSkills = new List<Skill>();
    public List<Skill> unlockedSkills = new List<Skill>();

    [Header("Other")]
    public bool isDied;
    public bool isInParty;


    public void LoadFromBase(CharacterData data)
    {
        //originData = data;
        _name = data.name;
        weaponType= data.weaponType;


        maxHealth = data.maxHealth;
        maxStamina = data.maxStamina;
        maxMana = data.maxMana;

        Regen();

        strength = data.strength;
        technical = data.technical;
        focus = data.focus;

        speed = data.speed;


        sprite = data.sprite;
        attack = data.attack;

        unlockedSkills = new List<Skill>(data.skills);
        foreach (Skill skill in unlockedSkills) TryEquipSkill(skill);
    }

    public void Regen()
    {
        isDied = false;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;
    }


    public void UnlockSkill(Skill skill)
    {
        //Debug.Log(skill.ToString());
        if (unlockedSkills.Contains(skill))
        {
            Debug.LogWarning("bu skill zaten öðrenilmiþ");
            return;
        }
        Debug.Log("Skill öðrenldi");
        unlockedSkills.Add(skill);
        TryEquipSkill(skill);
    }

    public void TryEquipSkill(Skill skill)
    {
        if (currentSkills.Count < 4)
        {
            //characterStats.isInParty = true;
            currentSkills.Add(skill);
        }
        else
        {
            Debug.LogWarning("skiller dolu");
        }
    }

    public void TryUnequipSkill(Skill skill)
    {
        if (currentSkills.Count > 1 && currentSkills.Contains(skill))
        {
            //characterToRemove.isInParty = false;
            currentSkills.Remove(skill);
        }

    }
}
