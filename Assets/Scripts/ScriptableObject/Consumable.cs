using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable Objects/Consumables")]
public abstract class Consumable : ScriptableObject
{
    public string _name;
    public Skill skill;
}
