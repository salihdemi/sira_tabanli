using UnityEditor;
using UnityEngine;

public class Talisman : Equipable
{
    public string talismanName;

    public virtual void OnTalismanEquipped(PersistanceStats owner) { }
    public virtual void OnTalismanUnequipped(PersistanceStats owner) { }

    public virtual void OnFightStart(Profile owner) { }
    public virtual void OnFightEnd(Profile owner) { }

    public virtual void OnTourStart(Profile owner) { }
    public virtual void OnTourEnd(Profile owner) { }

    public virtual void OnDealDamage(Profile owner, Profile dealer, float damage) { }//cagir!
    public virtual void OnTakeDamage(Profile owner, Profile dealer, float damage) { }

    public virtual void OnDie(Profile owner, Profile dealer, float damage) { }

    //onkill?

}