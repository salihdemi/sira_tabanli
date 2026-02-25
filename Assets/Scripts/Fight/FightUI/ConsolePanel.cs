using System.Threading;
using TMPro;
using UnityEngine;

public class ConsolePanel : MonoBehaviour
{
    public static ConsolePanel instance;

    public TextMeshProUGUI text;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            TurnScheduler.onStartPlay += Enable;
            TurnScheduler.onTourStart += Disable;
        }
        else Destroy(gameObject);
    }
    private void OnDestroy()
    {
        TurnScheduler.onStartPlay -= Enable;
        TurnScheduler.onTourStart -= Disable;
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
