using TMPro;
using UnityEngine;

public class ConsolePanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        TurnScheduler.onStartPlay += Enable;
        TurnScheduler.onTourLungesStart += Disable;
        Profile.OnSomeonePlay += WriteConsole;
    }
    private void OnDestroy()
    {
        TurnScheduler.onStartPlay -= Enable;
        TurnScheduler.onTourLungesStart -= Disable;
        Profile.OnSomeonePlay -= WriteConsole;
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void WriteConsole(string text)
    {
        //Enable();//ayýrmalý
        this.text.text = text;
    }
}
