using UnityEngine;
using UnityEngine.UI;

public class TalismanSelectCard : MonoBehaviour
{
    [HideInInspector] public Talisman talisman;
    public Button button;
    public Image image;
    public void OnCardClicked(PersistanceStats stats)
    {
        if (stats.talimsan != null) { InventoryManager.equippedTalismans.Remove(stats.talimsan); }

        stats.talimsan = talisman;
        InventoryManager.equippedTalismans.Add(talisman);
    }
}
