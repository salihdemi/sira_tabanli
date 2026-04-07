using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour, IInteractable
{
    public string ID;
    public List<DialogData> dialogs;
    public int dialogIndex;

    public void Interact()
    {
        if (DialogManager.Instance.IsOpen || dialogs == null || dialogs.Count == 0) return;
        DialogData current = dialogs[Mathf.Min(dialogIndex, dialogs.Count - 1)];
        if (dialogIndex < dialogs.Count - 1) dialogIndex++;
        DialogManager.Instance.StartDialog(current, OnChoiceSelected, OnLineAction);
    }

    private void OnChoiceSelected(DialogChoice choice) => ExecuteActions(choice.actions);

    private void OnLineAction(DialogLine line) => ExecuteActions(line.actions);

    private void ExecuteActions(System.Collections.Generic.List<DialogAction> actions)
    {
        foreach (var action in actions)
        {
            switch (action.actionType)
            {
                case DialogActionType.GiveItem:
                    (action.rewardObject as ICollectibleReward)?.Give();
                    break;
                case DialogActionType.StartFight:
                    if (action.enemies != null && action.enemies.Count > 0)
                        FightManager.StartFight(action.enemies);
                    break;
                case DialogActionType.ProgressStory:
                    break;
            }
        }
    }
}
