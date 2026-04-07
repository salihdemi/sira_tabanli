using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour, IInteractable
{
    public string ID;
    public List<DialogData> dialogs;
    public int dialogIndex;
    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    public void Interact()
    {
        if (DialogManager.Instance.IsOpen || dialogs == null || dialogs.Count == 0) return;
        FacePlayer();
        DialogData current = dialogs[Mathf.Min(dialogIndex, dialogs.Count - 1)];
        if (dialogIndex < dialogs.Count - 1) dialogIndex++;
        DialogManager.Instance.StartDialog(current, gameObject);
    }

    private void FacePlayer()
    {
        if (animator == null) return;
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Vector2 dir = player.transform.position - transform.position;
        float x, y;
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
        { x = dir.x > 0 ? 1 : -1; y = 0; }
        else
        { x = 0; y = dir.y > 0 ? 1 : -1; }

        animator.SetFloat("moveX", x);
        animator.SetFloat("moveY", y);
    }
}
