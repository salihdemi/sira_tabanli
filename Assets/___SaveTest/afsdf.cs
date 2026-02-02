using UnityEngine;
using UnityEngine.SceneManagement;

public class afsdf : MonoBehaviour
{
    public void ChangeSceneWithSave(int a)
    {
        SceneManager.LoadScene(a);
    }
    public void ChangeSceneWithoutSave(int a)
    {
        SceneManager.LoadScene(a);
    }
}
