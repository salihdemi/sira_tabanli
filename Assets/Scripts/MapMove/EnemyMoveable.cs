using System;
using System.Linq;
using UnityEngine;

public class EnemyMoveable : MapMoveable
{
    [SerializeField] public CharacterData[] datas; //!!!!!!!!!!
    [SerializeField] public PersistanceStats[] enemyStats;

    public static event Action <EnemyMoveable> OnSomeoneCollideMainCharacterMoveable;


    private bool trigger;

    private MainCharacterMoveable mainCharacter;


    public string loot;

    private void Awake()
    {
        for (int i = 0; i < enemyStats.Length; i++)
        {
            if (enemyStats.Length > i && datas.Length > i)
            {
                enemyStats[i].LoadFromBase(datas[i]);
            }
        }
    }


    protected override void Move()
    {
        if (!trigger || isInFight) return;
        float targetX = mainCharacter.transform.position.x;
        float targetY = mainCharacter.transform.position.y;

        float currentX = transform.position.x;
        float currentY = transform.position.y;
        //x uzaksa
        if (Mathf.Abs(currentX - targetX) >= Mathf.Abs(currentY - targetY))
        {
            if (y < 0f) { transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), 0f); }
            else { transform.position = new Vector3(transform.position.x, Mathf.Ceil(transform.position.y), 0f); }

            y = 0;

            x = Mathf.Sign(targetX - currentX);
        }
        //y uzaksa
        else
        {
            if (y < 0f) { transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, 0f); }
            else { transform.position = new Vector3(Mathf.Ceil(transform.position.x), transform.position.y, 0f); }

            x = 0;

            y = Mathf.Sign(targetY - currentY);
        }

        rb.linearVelocity = new Vector3(x * speed, y * speed, 0);
    }

    protected override void CheckStop()
    {
        if (isInFight)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    //Savaþa giriþ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<MapMoveable>(out MapMoveable character))
        {
            gameObject.SetActive(false);
            OnSomeoneCollideMainCharacterMoveable.Invoke(this);

        }
    }
    //Tetikleniþ
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!trigger && other.gameObject.TryGetComponent<MainCharacterMoveable>(out mainCharacter))
        {
            trigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (trigger && other.gameObject.GetComponent<MainCharacterMoveable>())
        {
            trigger = false;
            mainCharacter = null;
        rb.linearVelocity = Vector2.zero;
        }
    }
}
