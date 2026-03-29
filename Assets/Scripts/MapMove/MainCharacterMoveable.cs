using UnityEngine;

public class MainCharacterMoveable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public float speed = 10;
    private Vector2 moveInput;


    private bool isInFight;

    void OnEnable()
    {

        FightManager.OnFightStart += SetInFight;
        FightManager.OnFightEnd += SetNotInFight;
    }
    private void OnDisable()//ondestroydan cevirdim?
    {
        FightManager.OnFightStart -= SetInFight;
        FightManager.OnFightEnd -= SetNotInFight;
    }
    private void SetInFight() => isInFight = true;
    private void SetNotInFight() => isInFight = false;



    private void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(inputX, inputY).normalized;

        rb.linearVelocity = moveInput * speed * Time.deltaTime * 100;

        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) >= Mathf.Abs(moveInput.y))
            {
                animator.SetFloat("moveX", moveInput.x > 0 ? 1 : -1);
                animator.SetFloat("moveY", 0);
            }
            else
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", moveInput.y > 0 ? 1 : -1);
            }
        }
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    private void Update()
    {
        if (!isInFight) Move();
        else rb.linearVelocity = Vector2.zero;

        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("shoot");
    }
}