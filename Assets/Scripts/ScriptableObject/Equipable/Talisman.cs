using UnityEditor;
using UnityEngine;

public class Talisman : Equipable
{
    public string talismanName;
    public Sprite icon;

    public virtual void OnTalismanEquipped(PersistanceStats owner) { }
    public virtual void OnTalismanUnequipped(PersistanceStats owner) { }

    public virtual void OnTourStart(Profile owner, float damage) { }
    public virtual void OnTourEnd(Profile owner, float damage) { }

    public virtual void OnDealDamage(Profile owner, float damage) { }
    public virtual void OnTakeDamage(Profile owner, float damage) { }//Thorns

}