using UnityEngine;
using UnityEngine.SceneManagement;

public static class TransitionManager
{
    private static string nextSpawnID;


    public static void StartTransition(string sceneName, string spawnID)
    {
        nextSpawnID = spawnID;
        SceneManager.sceneLoaded += OnSceneLoaded; // Sahne yüklenince tetiklen
        SceneManager.LoadScene(sceneName);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Aboneliði hemen sil

        // Sahnede doðru SpawnPoint'i bul
        SpawnPoint[] points = GameObject.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        foreach (SpawnPoint sp in points)
        {
            if (sp.spawnID == nextSpawnID)
            {
                // Karakteri bul ve oraya ýþýnla
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = sp.transform.position;
                }
                break;
            }
        }
    }
}