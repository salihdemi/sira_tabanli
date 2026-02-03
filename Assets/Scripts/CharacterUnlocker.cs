using UnityEditor.SceneManagement;
using UnityEngine;

public class CharacterUnlocker : MonoBehaviour
{
    public CharacterData characterData;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("CharacterUnlockerOnCollisionEnter2D");
        if(characterData == null) return;
        PartyManager.UnlockAlly(characterData);
        Destroy(this);
    }

}
