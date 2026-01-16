using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersistanceStats
{
    [HideInInspector] public CharacterData originData;

    public float currentHealth;

    public float maxHealth;
    public float basePower;
    public float baseSpeed;

    [Header("Skills")]
    public _Skill attack;
    public List<_Skill> skills = new List<_Skill>();
    public void LoadFromBase(CharacterData data)
    {
        originData = data;
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        basePower = data.basePower;
        baseSpeed = data.baseSpeed;

        attack = data.attack;
        skills = new List<_Skill>(data.skills);
    }

    public void LearnSkill(_Skill skill)
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