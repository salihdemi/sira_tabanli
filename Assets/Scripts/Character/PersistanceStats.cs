using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersistanceStats
{
    //[HideInInspector] public CharacterData originData;

    public string _name;


    public float currentHealth;

    public float maxHealth;
    public float basePower;
    public float baseSpeed;
    public bool isDied;
    public bool isInParty;




    public Sprite sprite;
    [Header("Skills")]
    public Useable attack;
    public List<Useable> skills = new List<Useable>();



    public void LoadFromBase(CharacterData data)
    {
        //originData = data;
        _name = data.name;
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;
        basePower = data.basePower;
        baseSpeed = data.baseSpeed;


        sprite = data.sprite;
        attack = data.attack;
        skills = new List<Useable>(data.skills);
    }
    public void LoadFromSave()
    {

    }

    public void GetRest()
    {
        isDied = false;
        currentHealth = maxHealth;
    }


    public void LearnSkill(Useable skill)
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