using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] public EnemyMoveable[] moveables;
    [HideInInspector] public List<PersistanceStats> enemyStats;
    public bool trigger;

    public static event Action<EnemyGroup> OnSomeoneCollideMainCharacterMoveable;

    public string loot;
    private void Awake()
    {
        for (int i = 0; i < moveables.Length; i++)
        {
            if (moveables.Length > i)
            {
                PersistanceStats stat = new PersistanceStats();
                stat.LoadFromBase(moveables[i].data);
                enemyStats.Add(stat);
            }
        }
    }

    public void Cath()
    {
        foreach (EnemyMoveable moveable in moveables)
        {
            gameObject.SetActive(false);
        }
        OnSomeoneCollideMainCharacterMoveable.Invoke(this);
    }



    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!trigger && collision.GetComponent<MainCharacterMoveable>())
        {
            trigger = true;
            Debug.Log("grup mnziline girdi");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (trigger && collision.GetComponent<MainCharacterMoveable>())
        {
            // Tüm arkadaþlar takibi býraksýn
            foreach(EnemyMoveable moveable in moveables)
            {
                moveable.trigger = false;
            }
            trigger = false;
            Debug.Log("grup mnzilinden çýktý");
        }
    }
    */
}
