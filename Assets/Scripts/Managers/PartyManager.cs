using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] public AllyData[] party;
}
