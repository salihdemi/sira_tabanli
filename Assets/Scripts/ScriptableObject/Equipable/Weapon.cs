using UnityEngine;

public enum WeaponType
{
    a,b,c
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Equipables/Weapon")]
public class Weapon : Equipable
{
    public WeaponType type;
}
