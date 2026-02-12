using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectCard : MonoBehaviour
{
    [HideInInspector] public Weapon weapon;
    public Button button;
    public Image image;
    public void OnCardClicked(PersistanceStats stats)
    {
        if(stats.weapon != null){ InventoryManager.equippedWeapons.Remove(stats.weapon); }

        stats.weapon = weapon;
        InventoryManager.equippedWeapons.Add(weapon);
    }
}
