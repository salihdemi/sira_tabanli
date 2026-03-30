using System;
using UnityEngine;

public enum EnemyState { Idle, Chasing, InFight }

public class EnemyMoveable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public float speed = 10;
    [Header("Settings")]
    [SerializeField] private EnemyGroup group;
    [SerializeField] public CharacterData data;
    [SerializeField] private float stopDistance = 0.5f;

    public EnemyState currentState = EnemyState.Idle;
    private Transform mainCharacter;
    private Vector3 startPosition;

    [HideInInspector] public bool trigger; // Bireysel menzil kontrol

    private void OnEnable()
    {
        startPosition = transform.position;
        ChangeState(EnemyState.Idle);
        if (data.mapAnimatorController != null)
            animator.runtimeAnimatorController = data.mapAnimatorController;
    }
    public void ResetEnemy()
    {
        trigger = false;
        rb.linearVelocity = Vector2.zero;
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

    private void SetAnimator(Vector2 velocity)
    {
        bool isMoving = velocity != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
            {
                animator.SetFloat("moveX", velocity.x > 0 ? 1 : -1);
                animator.SetFloat("moveY", 0);
            }
            else
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", velocity.y > 0 ? 1 : -1);
            }
        }
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
        float distance = Vector2.Distance(transform.position, startPosition);

        if (distance > stopDistance)
        {
            Vector3 direction = (startPosition - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            SetAnimator(rb.linearVelocity);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            SetAnimator(Vector2.zero);
        }

        if (trigger && group.trigger && mainCharacter != null)
        {
            ChangeState(EnemyState.Chasing);
        }
    }

    private void HandleChasingState()
    {
        if (mainCharacter == null || !group.trigger || !trigger)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        float distance = Vector2.Distance(transform.position, mainCharacter.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (mainCharacter.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            SetAnimator(rb.linearVelocity);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            SetAnimator(Vector2.zero);
        }
    }

    private void HandleInFightState()
    {
        rb.linearVelocity = Vector2.zero;
        SetAnimator(Vector2.zero);
    }

    // --- TETIKLEYICILER ---

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentState != EnemyState.InFight)
            {
                group.Cath();
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
        }
    }
}
