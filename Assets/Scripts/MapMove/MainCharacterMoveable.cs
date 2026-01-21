using UnityEngine;

public class MainCharacterMoveable : MapMoveable
{
    private Vector2 moveInput;

    protected override void Move()
    {
        // Eðer savaþtaysak hareket etme
        if (isInFight)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 1. Input alma (Yatay ve Dikey)
        // GetAxisRaw kullanýyoruz ki hareket daha keskin/net olsun
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(inputX, inputY).normalized;



        // Tuþ býrakýldýðýnda veya input yoksa hýzý sýfýrla
        if (moveInput == Vector2.zero || isInFight)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!isInFight)
        {
            // 2. Fiziksel hareket
            // Serbest harekette rb.MovePosition yerine velocity (hýz) kullanmak 
            // duvarlarla etkileþim için genellikle daha pürüzsüzdür.
            rb.linearVelocity = moveInput * speed;
        }
    }
}