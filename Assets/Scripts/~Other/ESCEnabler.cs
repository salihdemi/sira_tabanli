using UnityEngine;

public class ESCEnabler : MonoBehaviour
{
    public GameObject _gameObject;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnScheduler.Back();
            Debug.Log("Space tuþuna basýldý!");
        }



        if (_gameObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_gameObject.activeSelf)
                {
                    _gameObject.SetActive(false);
                }
                else
                {
                    _gameObject.SetActive(true);
                }
            }
        }
    }
}
