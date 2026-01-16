using TMPro;
using UnityEngine;

public class ConsolePanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        TurnScheduler.onStartPlay += () => gameObject.SetActive(true);
        TurnScheduler.onStartTour += () => gameObject.SetActive(false);
        Profile.OnSomeonePlay += WriteText;
    }
    private void OnDestroy()
    {
        TurnScheduler.onStartPlay -= () => gameObject.SetActive(true);
        TurnScheduler.onStartTour -= () => gameObject.SetActive(false);
        Profile.OnSomeonePlay -= WriteText;
    }
    public void WriteText(string text)
    {
        gameObject.SetActive(true);//ayýrmalý
        this.text.text = text;
    }
}
