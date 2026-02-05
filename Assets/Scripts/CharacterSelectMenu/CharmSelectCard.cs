using UnityEngine;
using UnityEngine.UI;

public class CharmSelectCard : MonoBehaviour
{
    [HideInInspector] public Charm charm;
    public Button button;
    public Image image;
    public void OnCardClicked(PersistanceStats stats)
    {
        if (stats.charm != null) { stats.charm.equipped = false; }

        stats.charm = charm;
        charm.equipped = true;
    }
}
