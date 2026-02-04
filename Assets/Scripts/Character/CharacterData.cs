
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    
    [Header("Stats")]
    public float maxHealth;
    public float basePower;
    public float baseSpeed;
    //stamina
    //mana
    //kalkan
    //zýrh




    [Header("Visuals")]
    public Sprite sprite;




    [Header("Skills")]
    public Skill attack;
    public List<Skill> skills = new List<Skill>();








}
