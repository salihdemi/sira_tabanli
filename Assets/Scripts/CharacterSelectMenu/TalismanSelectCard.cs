using UnityEngine;
using UnityEngine.UI;

public class TalismanSelectCard : MonoBehaviour
{
    [HideInInspector] public Talisman talisman;
    public Button button;
    public Image image;
    public void OnCardClicked(PersistanceStats stats)
    {
        if (stats.talimsan != null)
        {
            stats.talimsan.OnTalismanUnequipped(stats);
            InventoryManager.equippedTalismans.Remove(stats.talimsan);
        }

        stats.talimsan = talisman;
        talisman.OnTalismanEquipped(stats);
        InventoryManager.equippedTalismans.Add(talisman);
    }
}
