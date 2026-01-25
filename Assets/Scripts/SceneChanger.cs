using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int sceneToTeleport;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(sceneToTeleport);
    }
}
