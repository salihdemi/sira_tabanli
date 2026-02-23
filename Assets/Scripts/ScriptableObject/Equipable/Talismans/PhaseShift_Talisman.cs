using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Kalkanlý düþmanlarda tetiklenmez
[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/PhaseShift_Talisman")]
public class PhaseShift_Talisman : Talisman
{
    public float damageThreshold = 20f; // Tek seferde alýnmasý gereken sýnýr hasar
    public Sprite transformedSprite;    // Dönüþeceði yeni görsel

    public override void OnTakeDamage(Profile owner, Profile dealer, float damage)
    {
        if (damage >= damageThreshold)
        {
            // Dönüþüm sürecini sýraya sokalým (CombatManager kullanarak)
            CombatManager.AddAction(TriggerTransformation(owner));
        }
    }

    private IEnumerator TriggerTransformation(Profile owner)
    {
        string log = owner.name + " dönüþüyor";
        ConsolePanel.instance.WriteConsole(log);

        owner.GetComponent<Image>().sprite = transformedSprite;

        owner.AddToStrength(5);
        owner.AddShield(20);

        yield return new WaitForSeconds(1);
    }
}