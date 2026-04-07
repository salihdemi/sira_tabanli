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
        if (DialogManager.Instance.IsOpen) return;
        DialogData dataToShow = dialog != null ? dialog : CreateDefaultDialog();
        DialogManager.Instance.StartDialog(dataToShow, gameObject);
    }

    private DialogData CreateDefaultDialog()
    {
        DialogData data = ScriptableObject.CreateInstance<DialogData>();
        DialogLine line = new DialogLine();
        line.text = pickupMessage;
        line.choices = new System.Collections.Generic.List<DialogChoice>
        {
            new DialogChoice { choiceText = "Al", actions = new System.Collections.Generic.List<DialogAction> { new DialogAction { actionType = DialogActionType.GiveItem } } },
            new DialogChoice { choiceText = "Alma" }
        };
        data.lines.Add(line);
        return data;
    }

    public void Give()
    {
        reward?.Give();

        SaveManager.RegisterCollected(ID);
        Destroy(gameObject);
    }
}
