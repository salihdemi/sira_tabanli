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

        FightManager.OnFightStart += SetInFight;
        FightManager.OnFightEnd += SetNotInFight;
    }
    private void OnDestroy()
    {
        FightManager.OnFightStart -= SetInFight;
        FightManager.OnFightEnd -= SetNotInFight;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void Move();
    void Update()
    {
        Move();


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
