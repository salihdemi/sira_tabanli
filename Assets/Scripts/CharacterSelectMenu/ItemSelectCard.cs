using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSelectCard : MonoBehaviour
{
    [HideInInspector] public Item item;
    public Button button;
    public Image image;
    public void OnCardClicked(PersistanceStats stats)
    {
        if (stats.item != null) { InventoryManager.equippedItems.Remove(stats.item); }

        stats.item = item;
        InventoryManager.equippedItems.Add(item);
    }
}
