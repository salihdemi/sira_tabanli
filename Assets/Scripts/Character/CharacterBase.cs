
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBase : ScriptableObject
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
    public Sprite _sprite;


    [Header("Skills")]
    public _Skill attack;
    public List<_Skill> skills = new List<_Skill>();











}
