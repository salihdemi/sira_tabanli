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
            TurnScheduler.onTourLungesStart += Disable;
        }
        else Destroy(gameObject);
    }
    private void OnDestroy()
    {
        TurnScheduler.onStartPlay -= Enable;
        TurnScheduler.onTourLungesStart -= Disable;
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
    /*
    public void PlayIfAlive(Profile profile)
    {
        string text;
        if (profile.isDied)
        {
            profile.lastTargetName =
            text = name + " " + profile.lastTargetName + "'a vurmadý çünkü " + name + " öldü";
        }
        else if (profile.currentTarget && profile.currentTarget.isDied)
        {
            text = name + " " + profile.lastTargetName + "'a vurmadý çünkü " + profile.lastTargetName + " öldü";
        }
        else if (profile.mute > 0)
        {
            text = name + " susturuldu";
        }
        else
        {
            text = name + " " + profile.lastTargetName + "'a " + profile.currentUseable.name + " yaptý";
            profile.Play();
        }
        //Debug.Log(text);
        WriteConsole(text);//WriteConsole
    }
    */
}
