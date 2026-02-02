using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    public SaveSlotUI[] slots;

    void OnEnable()
    {
        // Eðer slots dizisi boþsa, alt objelerdeki tüm slotlarý otomatik bulur
        if (slots == null || slots.Length == 0)
        {
            slots = GetComponentsInChildren<SaveSlotUI>();
        }

        foreach (SaveSlotUI slot in slots)
        {
            if (slot != null) slot.Setup();
        }
    }


}