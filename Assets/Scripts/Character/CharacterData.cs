
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    
    [Header("Stats")]
    public float maxHealth, maxStamina, maxMana;
    public float strength, technical, focus;
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
