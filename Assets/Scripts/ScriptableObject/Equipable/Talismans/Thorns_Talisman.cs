using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/Thorns")]
public class Thorns_Talisman : Talisman
{
    public float reflectDamage = 5f;

    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        string log = owner.name + " hasarý yansýttý";
        ConsolePanel.instance.WriteConsole(log);
        CombatManager.AddAction(ads(owner, dealer));


    }
    private IEnumerator ads(Profile owner, Profile dealer)
    {
        dealer.AddToHealth(-reflectDamage, owner);
        yield return new WaitForSeconds(1);
    }




}