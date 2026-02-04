using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersistanceStats
{
    //[HideInInspector] public CharacterData originData;

    public string _name;


    public float currentHealth, currentStamina, currentMana;

    public bool isDied;
    public bool isInParty;


    [Header("Stats")]
    public float maxHealth, maxStamina, maxMana;
    public float strength, technical, focus; 

    public float baseSpeed;


    public Sprite sprite;
    [Header("Skills")]
    public Skill attack;
    public List<Skill> skills = new List<Skill>();



    public void LoadFromBase(CharacterData data)
    {
        //originData = data;
        _name = data.name;
        maxHealth = data.maxHealth;
        maxStamina = data.maxHealth;
        maxMana = data.maxHealth;

        Regen();

        strength = data.strength;
        technical = data.technical;
        focus = data.focus;

        baseSpeed = data.baseSpeed;


        sprite = data.sprite;
        attack = data.attack;
        skills = new List<Skill>(data.skills);
    }

    public void Regen()
    {
        isDied = false;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;
    }


    public void LearnSkill(Skill skill)
    {
        Debug.Log(skill.ToString());
        if (skills.Contains(skill))
        {
            Debug.Log("bu skill zaten öðrenilmiþ");
            return;
        }
        Debug.Log("Skill öðrenldi");
        skills.Add(skill);
    }
    
}