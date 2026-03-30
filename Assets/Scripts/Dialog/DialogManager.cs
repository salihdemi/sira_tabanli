using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    public bool IsOpen => panel.activeSelf;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private GameObject choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Settings")]
    [SerializeField] private float typeSpeed = 0.03f;

    private DialogData currentDialog;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;
    private System.Action<DialogChoice> onChoiceSelected;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    private void Update()
    {
        if (!panel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
                CompleteTyping();
            else if (HasChoices())
                return; // seçenek varken E ile geçme, buton bekle
            else
                NextLine();
        }
    }

    public void StartDialog(DialogData data, System.Action<DialogChoice> choiceCallback = null)
    {
        currentDialog = data;
        currentLineIndex = 0;
        onChoiceSelected = choiceCallback;
        panel.SetActive(true);
        ShowLine(currentDialog.lines[currentLineIndex]);
    }

    private void ShowLine(DialogLine line)
    {
        speakerText.text = line.speakerName;
        portraitImage.sprite = line.portrait;
        portraitImage.gameObject.SetActive(line.portrait != null);

        ClearChoices();

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line.text, line));
    }

    private IEnumerator TypeText(string text, DialogLine line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;

        if (line.choices.Count > 0)
            ShowChoices(line.choices);
    }

    private void CompleteTyping()
    {
        StopCoroutine(typingCoroutine);
        isTyping = false;
        dialogText.text = currentDialog.lines[currentLineIndex].text;

        var line = currentDialog.lines[currentLineIndex];
        if (line.choices.Count > 0)
            ShowChoices(line.choices);
    }

    private void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialog.lines.Count)
        {
            CloseDialog();
            return;
        }
        ShowLine(currentDialog.lines[currentLineIndex]);
    }

    private void ShowChoices(List<DialogChoice> choices)
    {
        choicesContainer.SetActive(true);
        foreach (DialogChoice choice in choices)
        {
            GameObject btn = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
            DialogChoice captured = choice;
            btn.GetComponent<Button>().onClick.AddListener(() => OnChoiceClicked(captured));
        }
    }

    private void OnChoiceClicked(DialogChoice choice)
    {
        ClearChoices();
        onChoiceSelected?.Invoke(choice);

        if (choice.nextDialog != null)
            StartDialog(choice.nextDialog, onChoiceSelected);
        if (choice.actionType != DialogActionType.CollectItem)
        {
            
        }
        else
            CloseDialog();
    }

    private void ClearChoices()
    {
        choicesContainer.SetActive(false);
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
    }

    private bool HasChoices() =>
        currentLineIndex < currentDialog.lines.Count &&
        currentDialog.lines[currentLineIndex].choices.Count > 0;

    public void CloseDialog()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        ClearChoices();
        panel.SetActive(false);
    }
}
