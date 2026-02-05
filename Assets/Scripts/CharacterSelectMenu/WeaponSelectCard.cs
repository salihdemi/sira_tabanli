using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectCard : MonoBehaviour
{
    [HideInInspector] public Weapon weapon;
    public Button button;
    public Image image;
    public void OnCardClicked(PersistanceStats stats)
    {
        if(stats.weapon != null){ stats.weapon.equipped = false; }

        stats.weapon = weapon;
        weapon.equipped = true;
    }
}
