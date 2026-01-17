using TMPro;
using UnityEngine;

public class ConsolePanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        TurnScheduler.onStartPlay += Enable;
        TurnScheduler.onStartTour += Disable;
        Profile.OnSomeonePlay += WriteText;
    }
    private void OnDestroy()
    {
        TurnScheduler.onStartPlay -= Enable;
        TurnScheduler.onStartTour -= Disable;
        Profile.OnSomeonePlay -= WriteText;
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void WriteText(string text)
    {
        gameObject.SetActive(true);//ayýrmalý
        this.text.text = text;
    }
}
