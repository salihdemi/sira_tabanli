using UnityEngine;

public class MainCharacterMoveable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
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
    }

    private void Update()
    {
        if (!isInFight) Move();
        else rb.linearVelocity = Vector2.zero;
    }
}