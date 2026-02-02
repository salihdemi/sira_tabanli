using UnityEngine;


public class FirstPartyUnlocker : MonoBehaviour
{
    public CharacterData[] partyDatas;
    private void Awake()
    {
        for (int i = 0; i < partyDatas.Length; i++)
        {
            PartyManager.UnlockAlly(partyDatas[i]);
        }
    }
}
