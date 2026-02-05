using UnityEngine;

public class TestStarter : MonoBehaviour
{
    [SerializeField] private CharacterData[] partyDatas;
    private void Awake()
    {
        for (int i = 0; i < partyDatas.Length; i++)
        {
            PartyManager.UnlockAlly(partyDatas[i]);
        }
    }
}
