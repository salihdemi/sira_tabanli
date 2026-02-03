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


        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Setup(i);
        }
    }


}