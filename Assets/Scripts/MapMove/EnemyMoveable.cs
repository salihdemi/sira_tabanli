using UnityEngine;

public class EnemyMoveable : MapMoveable
{
    [SerializeField] private EnemyData[] enemies;




    private bool trigger;
    protected override void Move()
    {
        float targetX = MainCharacterMoveable.instance.transform.position.x;
        float targetY = MainCharacterMoveable.instance.transform.position.y;

        float currentX = transform.position.x;
        float currentY = transform.position.y;
        if(trigger)
        {
            //x uzaksa
            if (Mathf.Abs(currentX - targetX) >= Mathf.Abs(currentY - targetY))
            {
                if (y < 0f) { transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), 0f); }
                else        { transform.position = new Vector3(transform.position.x, Mathf.Ceil (transform.position.y), 0f); }

                y = 0;

                x = Mathf.Sign(targetX - currentX);
            }
            //y uzaksa
            else
            {
                if (y < 0f) { transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, 0f); }
                else        { transform.position = new Vector3(Mathf.Ceil (transform.position.x), transform.position.y, 0f); }

                x = 0;

                y = Mathf.Sign(targetY - currentY);
            }

            rb.linearVelocity = new Vector3(x * speed, y * speed, 0);
        }
    }

    protected override void CheckStop()
    {

    }

    //Savaþa giriþ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<MapMoveable>(out MapMoveable character))
        {
            gameObject.SetActive(false);
            FightManager.instance.StartFight(PartyManager.instance.party, enemies);
        }
    }
    //Tetikleniþ
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!trigger && other.gameObject.GetComponent<MainCharacterMoveable>())
        {
            trigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (trigger && other.gameObject.GetComponent<MainCharacterMoveable>())
        {
            trigger = false;
        }
    }
}
