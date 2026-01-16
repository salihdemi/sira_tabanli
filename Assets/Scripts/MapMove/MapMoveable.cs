using TMPro;
using UnityEngine;

public abstract class MapMoveable : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float speed = 10;


    protected float x, y;



    protected bool isInFight;

    void Awake()
    {

        FightManager.OnFightStart += () => SetIsInFight(true);
        FightManager.OnFightEnd += () => SetIsInFight(false);
    }
    private void OnDestroy()
    {
        FightManager.OnFightStart -= () => SetIsInFight(true);
        FightManager.OnFightEnd -= () => SetIsInFight(false);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void Move();
    protected abstract void CheckStop();
    void Update()
    {
        Move();

        CheckStop();

    }
    private void SetIsInFight(bool can)
    {
        isInFight = can;
    }
}
