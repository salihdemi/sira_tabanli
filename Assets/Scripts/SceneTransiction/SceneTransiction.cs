using System.Transactions;
using UnityEngine;

public class SceneTransiction : MonoBehaviour
{
    public string targetSceneName; // Gidilecek sahne
    public string targetSpawnID;   // Yeni sahnedeki lokasyonun adý (Örn: "Door_A")

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Geçiþ bilgilerini bir yere kaydet ve sahneyi yükle
            TransitionManager.StartTransition(targetSceneName, targetSpawnID);
        }
    }
}