using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID; // Kapýdaki ID ile ayný olmalý

    private void OnDrawGizmos()
    {
        // Editörde neresi olduðunu görmek için
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}