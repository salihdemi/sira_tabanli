using UnityEngine;

public interface ICollectibleReward
{
    void Give();
}

public class Collectible : MonoBehaviour, IInteractable
{
    public string ID;
    public DialogData dialog;
    public string pickupMessage = "Bu eşyayı almak ister misin?";
    [SerializeField] private ScriptableObject rewardObject;
    private ICollectibleReward reward => rewardObject as ICollectibleReward;

    private void Awake()
    {
        if (rewardObject != null && reward == null)
            Debug.LogWarning($"{name}: Reward object ICollectibleReward implement etmiyor!");
    }

    public void Interact()
    {
        DialogData dataToShow = dialog != null ? dialog : CreateDefaultDialog();
        DialogManager.Instance.StartDialog(dataToShow, OnChoiceSelected);
    }

    private DialogData CreateDefaultDialog()
    {
        DialogData data = ScriptableObject.CreateInstance<DialogData>();
        DialogLine line = new DialogLine();
        line.text = pickupMessage;
        line.choices.Add(new DialogChoice { choiceText = "Al", actionType = DialogActionType.CollectItem });
        line.choices.Add(new DialogChoice { choiceText = "Alma", actionType = DialogActionType.None });
        data.lines.Add(line);
        return data;
    }

    private void OnChoiceSelected(DialogChoice choice)
    {
        if (choice.actionType == DialogActionType.CollectItem)
            Give();
    }

    public void Give()
    {
        reward?.Give();

        SaveManager.RegisterCollected(ID);
        Destroy(gameObject);
    }
}
