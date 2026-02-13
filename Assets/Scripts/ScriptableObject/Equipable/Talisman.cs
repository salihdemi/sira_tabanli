using UnityEditor;
using UnityEngine;

public class Talisman : Equipable
{
    public string talismanName;
    public Sprite icon;

    public virtual void OnTakeDamage(Profile owner, float damage) { }
}