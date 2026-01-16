using TMPro;
using UnityEngine;

public class ConsolePanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Awake()
    {
        Profile.OnSomeonePlay += WriteText;
    }
    private void OnDestroy()
    {
        Profile.OnSomeonePlay -= WriteText;
    }
    public void WriteText(string text)
    {
        gameObject.SetActive(true);//ayýrmalý
        this.text.text = text;
    }
}
