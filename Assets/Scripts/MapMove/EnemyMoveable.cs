using System;
using UnityEngine;

public enum EnemyState { Idle, Chasing, InFight }

public class EnemyMoveable : MapMoveable
{
    [Header("Settings")]
    [SerializeField] private EnemyGroup group;
    [SerializeField] public CharacterData data;
    [SerializeField] private float stopDistance = 0.5f;

    public EnemyState currentState = EnemyState.Idle;
    private Transform mainCharacter;
    private Vector3 startPosition;

    [HideInInspector] public bool trigger; // Bireysel menzil kontrolü

    private void OnEnable()
    {
        startPosition = transform.position;
        ChangeState(EnemyState.Idle);
    }
    public void ResetEnemy()
    {
        trigger = false;                    // Takip trigger'ýný sýfýrla

        rb.linearVelocity = Vector2.zero;   // Hareketini kes
    }

    private void Update()
    {
        HandleState();
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Chasing:
                HandleChasingState();
                break;
            case EnemyState.InFight:
                HandleInFightState();
                break;
        }
    }

    private void HandleIdleState()
    {
        rb.linearVelocity = Vector2.zero;


        float distance = Vector2.Distance(transform.position, startPosition);

        if (distance > stopDistance)
        {
            Vector3 direction = (startPosition - transform.position).normalized;
            // Fizik motoru (RB) kullanýrken Time.deltaTime kullanmana gerek yoktur, 
            // hýz (velocity) zaten zamandan baðýmsýz bir deðerdir.
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }


        // ÇÝFT KONTROL: Hem grup tetiklenmiþ olmalý hem de düþman oyuncuyu görmeli
        if (trigger && group.trigger && mainCharacter != null)
        {
            ChangeState(EnemyState.Chasing);
        }
    }

    private void HandleChasingState()
    {
        // ÞARTLARDAN BÝRÝ BOZULURSA (Grup menzilinden çýkýþ veya bireysel menzilden çýkýþ)
        if (mainCharacter == null || !group.trigger || !trigger)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        float distance = Vector2.Distance(transform.position, mainCharacter.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (mainCharacter.position - transform.position).normalized;
            // Fizik motoru (RB) kullanýrken Time.deltaTime kullanmana gerek yoktur, 
            // hýz (velocity) zaten zamandan baðýmsýz bir deðerdir.
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleInFightState()
    {
        rb.linearVelocity = Vector2.zero;
    }

    // --- TETÝKLEYÝCÝLER ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(currentState != EnemyState.InFight)
            {
                group.Cath(); // Bu fonksiyon tüm grubu InFight'a sokar
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trigger = true;
            mainCharacter = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trigger = false;
            // Chasing içindeki kontrol sayesinde otomatik Idle'a düþecek
        }
    }
}