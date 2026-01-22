using UnityEngine;

public class MainCharacterMoveable : MapMoveable
{
    private Vector2 moveInput;

    private void Move()
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
        if (moveInput == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
        }

        rb.linearVelocity = moveInput * speed * Time.deltaTime * 100;
    }

    private void Update()
    {
        Move();
    }
}