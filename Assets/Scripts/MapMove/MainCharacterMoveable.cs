using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MainCharacterMoveable : MapMoveable
{
    public static MainCharacterMoveable instance;
    MainCharacterMoveable() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] public AllyData[] party;
    protected override void Move()
    {
             if (Input.GetKey(KeyCode.W)) { x = 0; y = +1; }
        else if (Input.GetKey(KeyCode.S)) { x = 0; y = -1; }
        else if (Input.GetKey(KeyCode.D)) { y = 0; x = +1; }
        else if (Input.GetKey(KeyCode.A)) { y = 0; x = -1; }


        rb.linearVelocity = new Vector3(x * speed, y * speed, 0);
    }
    protected override void CheckStop()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            y = 0;
            transform.position = new Vector3(transform.position.x, Mathf.Ceil(transform.position.y), 0);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            y = 0;
            transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), 0);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            x = 0;
            transform.position = new Vector3(Mathf.Ceil(transform.position.x), transform.position.y, 0);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            x = 0;
            transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, 0);
        }
    }
}
