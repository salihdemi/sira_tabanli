
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public WeaponType weaponType;

    [Header("Bars")]
    public float maxHealth;
    public float maxStamina;
    public float maxMana;

    [Header("Stats")]
    public float strength;
    public float technical;
    public float focus;
    public float speed;

    [Header("Visuals")]
    public Sprite sprite;

    [Header("Skills")]
    public Skill attack;
    public List<Skill> skills = new List<Skill>();








}
