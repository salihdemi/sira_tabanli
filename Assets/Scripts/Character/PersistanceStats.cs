using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class PersistanceStats
{
    [Header("Name")]
    public string _name;

    [Header("Status")]
    public float currentHealth, currentStamina, currentMana;

    [Header("Stats")]
    public float maxHealth, maxStamina, maxMana;
    public float strength, technical, focus, baseSpeed;


    [Header("Sprite")]
    public Sprite sprite;

    [Header("Skills")]
    public Skill attack;
    public List<Skill> skills = new List<Skill>();

    [Header("Other")]
    public bool isDied;
    public bool isInParty;


    public void LoadFromBase(CharacterData data)
    {
        //originData = data;
        _name = data.name;
        maxHealth = data.maxHealth;
        maxStamina = data.maxStamina;
        maxMana = data.maxMana;

        Regen();

        strength = data.strength;
        technical = data.technical;
        focus = data.focus;

        baseSpeed = data.speed;


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
