using System;
using System.Linq;
using UnityEngine;

public class EnemyMoveable : MapMoveable
{
    [SerializeField] EnemyGroup group;

    [SerializeField] public CharacterData data;



    public bool trigger;

    private MainCharacterMoveable mainCharacter;





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



        Debug.Log(isInFight +" "+ !trigger + " "+ !group.trigger);

        if (isInFight || !trigger || !group.trigger)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }


    //Oyuncuyu yakalayýnca
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MainCharacterMoveable>())
        {
            group.Cath();
        }
    } 
    //Oyuncu menzile girince
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!trigger && group.trigger && other.gameObject.TryGetComponent<MainCharacterMoveable>(out mainCharacter))
        {
            trigger = true;
            Debug.Log("menzile girdi");
        }
    }
    //Oyuncu menzilden çýkýnca
    private void OnTriggerExit2D(Collider2D other)
    {
        if (trigger && other.gameObject.GetComponent<MainCharacterMoveable>())
        {
            trigger = false;
            Debug.Log("menzilden cýkrý");
        }
    }
}
