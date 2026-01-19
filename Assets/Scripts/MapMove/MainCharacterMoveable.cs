using UnityEngine;

public class MainCharacterMoveable : MapMoveable
{

    [Header("Grid Settings")]
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask obstacleLayer; // Duvarlarý seçmek için

    private Vector3 targetPosition;
    private bool isMoving = false;



    protected override void Move()
    {
        // Eðer zaten hareket halindeyse yeni input alma (Grid kuralý)
        if (isMoving || isInFight) return;

        float inputX = 0;
        float inputY = 0;

        // Input alma (Önce yatay sonra dikey kontrolü çapraz gitmeyi engeller)
        if (Input.GetKey(KeyCode.D)) inputX = 1;
        else if (Input.GetKey(KeyCode.A)) inputX = -1;
        else if (Input.GetKey(KeyCode.W)) inputY = 1;
        else if (Input.GetKey(KeyCode.S)) inputY = -1;

        if (inputX != 0 || inputY != 0)
        {
            Vector3 direction = new Vector3(inputX, inputY, 0);
            TryMove(direction);
        }
    }

    private void TryMove(Vector3 direction)
    {
        Vector3 potentialTarget = targetPosition + (direction * gridSize);

        // Hedef noktada engel var mý kontrol et (OverlapCircle)
        // Eðer duvarlarýn Layer'ýný 'obstacleLayer' olarak ayarlarsan burasý çalýþýr
        if (!Physics2D.OverlapCircle(potentialTarget, 0.2f, obstacleLayer))
        {
            targetPosition = potentialTarget;
            isMoving = true;
        }
    }

    // MapMoveable içindeki Update her kare çalýþýyor, biz fiziði FixedUpdate ile destekleyelim
    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Karakteri hedefe pürüzsüzce kaydýr
            Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // Hedefe ulaþtý mý kontrol et
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Tam kareye sabitle (Snap)
                isMoving = false;
            }
        }
    }

    protected override void CheckStop()
    {
        // Yeni sistemde tuþ býrakýldýðýnda ýþýnlama yapmaya gerek yok. 
        // Karakter zaten targetPosition'a vardýðýnda otomatik duruyor.
        // Bu metod MapMoveable zorunlu kýldýðý için boþ býrakýlabilir.
        if (!isMoving || isInFight)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

}