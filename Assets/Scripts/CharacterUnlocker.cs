using UnityEditor.SceneManagement;
using UnityEngine;

public class CharacterUnlocker : MonoBehaviour
{
    public PartyManager partyManager;
    public CharacterData characterData;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(characterData == null) return;
        partyManager.UnlockAlly(characterData);
        Destroy(this);
    }

}
