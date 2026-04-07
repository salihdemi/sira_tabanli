using UnityEngine;

public class MainCharacterMoveable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform interactionBox;
    public float speed = 10;
    private Vector2 moveInput;
    private Vector2 lastDir = Vector2.down;
    private Vector2 activeDir = Vector2.down;


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
        if (Input.GetKeyDown(KeyCode.W)) { activeDir = Vector2.up; }
        else if (Input.GetKeyDown(KeyCode.S)) { activeDir = Vector2.down; }
        else if (Input.GetKeyDown(KeyCode.A)) { activeDir = Vector2.left; }
        else if (Input.GetKeyDown(KeyCode.D)) { activeDir = Vector2.right; }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (activeDir.x != 0) moveInput = inputX != 0 ? new Vector2(inputX, 0) : Vector2.zero;
        else moveInput = inputY != 0 ? new Vector2(0, inputY) : Vector2.zero;

        // aktif tuş bırakıldıysa diğer basılı tuşa geç
        if (moveInput == Vector2.zero)
        {
            if (inputX != 0) { activeDir = new Vector2(inputX, 0); moveInput = activeDir; }
            else if (inputY != 0) { activeDir = new Vector2(0, inputY); moveInput = activeDir; }
        }

        rb.linearVelocity = moveInput * speed * Time.deltaTime * 100;

        Vector2 animDir = moveInput != Vector2.zero ? moveInput : lastDir;
        if (moveInput != Vector2.zero) { lastDir = moveInput; UpdateInteractionBox(moveInput); }
        animator.SetFloat("moveX", animDir.x);
        animator.SetFloat("moveY", animDir.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }

    private void UpdateInteractionBox(Vector2 dir)
    {
        if (interactionBox == null) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        interactionBox.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        bool isDialogOpen = DialogManager.Instance != null && DialogManager.Instance.IsOpen;

        if (!isInFight && !isDialogOpen) Move();
        else rb.linearVelocity = Vector2.zero;

        if (Input.GetMouseButtonDown(0) && !isDialogOpen && !IsPointerOverUI())
            animator.SetTrigger("shoot");
    }

    private bool IsPointerOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current != null &&
               UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}