using TMPro;
using UnityEngine;

public abstract class MapMoveable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    public float speed = 10;





    protected bool isInFight;

    void Awake()
    {

        FightManager.OnFightStart += SetInFight;
        FightManager.OnFightEnd += SetNotInFight;
    }
    private void OnDestroy()
    {
        FightManager.OnFightStart -= SetInFight;
        FightManager.OnFightEnd -= SetNotInFight;
    }

    private void SetInFight()
    {
        isInFight = true;
    }
    private void SetNotInFight()
    {
        isInFight = false;
    }
}
