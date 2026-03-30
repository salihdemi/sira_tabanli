using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public DialogData dialog;

    public void Interact()
    {
        if (dialog == null) return;
        DialogManager.Instance.StartDialog(dialog);
    }
}
