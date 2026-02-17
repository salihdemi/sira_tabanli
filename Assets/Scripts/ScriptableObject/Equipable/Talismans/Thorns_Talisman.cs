using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/Thorns")]
public class Thorns_Talisman : Talisman
{
    public float reflectDamage = 5f;

    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        dealer.AddToHealth(-reflectDamage, owner);
        Debug.Log($"Düþmana {reflectDamage} hasar yansýtýldý!");
        // Buraya düþmana hasar veren kodunu ekleyebilirsin
    }
}