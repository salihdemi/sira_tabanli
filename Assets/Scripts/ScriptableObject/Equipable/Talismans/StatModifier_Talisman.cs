using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Equipables/Talismans/StatModifier_Talisman")]
public class StatModifier_Talisman : Talisman
{
    public float healthBonus;
    public float manaBonus;
    public float staminaBonus;
    public float strengthBonus;
    public float technicalBonus;
    public float focusBonus;
    public float speedBonus;

    public override void OnTalismanEquipped(PersistanceStats owner)
    {
        owner.maxHealth += healthBonus;
        owner.maxMana += manaBonus;
        owner.maxStamina += staminaBonus;
        owner.strength += strengthBonus;
        owner.technical += technicalBonus;
        owner.focus += focusBonus;
        owner.speed += speedBonus;
    }
    public override void OnTalismanUnequipped(PersistanceStats owner)
    {
        owner.maxHealth -= healthBonus;
        owner.maxMana -= manaBonus;
        owner.maxStamina -= staminaBonus;
        owner.strength -= strengthBonus;
        owner.technical -= technicalBonus;
        owner.focus -= focusBonus;
        owner.speed -= speedBonus;
    }

}
