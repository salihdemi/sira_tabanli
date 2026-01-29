using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    public string groupID;
    [SerializeField] public EnemyMoveable[] moveables;
    [HideInInspector] public List<PersistanceStats> enemyStats = new List<PersistanceStats>(); // Liste oluþturuldu (Null hatasý engellendi)
    public bool trigger;



    public static List<EnemyGroup> GroupsInScene = new List<EnemyGroup>();
    public static event Action<EnemyGroup> OnSomeoneCollideMainCharacterMoveable;
    public string loot;
    private void Awake()
    {
        GroupsInScene.Add(this);
        SetUpEnemies();
    }

    private void SetUpEnemies()
    {
        // Grup içindeki düþmanlarýn verilerini hazýrla
        foreach (var enemy in moveables)
        {
            if (enemy.data != null)
            {
                PersistanceStats stat = new PersistanceStats();
                stat.LoadFromBase(enemy.data);
                enemyStats.Add(stat);
            }
        }
    }

    void OnDestroy()
    {
        GroupsInScene.Remove(this);
    }

    public void Cath()
    {
        // Savaþ baþladýðýnda gruptaki herkesi durdur
        foreach (EnemyMoveable moveable in moveables)
        {
            moveable.ChangeState(EnemyState.InFight);
        }
        OnSomeoneCollideMainCharacterMoveable?.Invoke(this);
    }
    public void ResetGroup()
    {
        // Grubu tekrar görünür yap
        gameObject.SetActive(true);
        trigger = false;

        // Ýçindeki her bir düþmaný baþlangýç pozisyonuna al ve canlarýný tazele
        foreach (EnemyMoveable moveable in moveables)
        {
            moveable.ResetEnemy();
        }

        // PersistanceStats (canlar vb.) listesini yeniden doldur veya tazele
        foreach (var stat in enemyStats)
        {
            stat.currentHealth = stat.maxHealth;
            stat.isDied = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trigger = true;
            // Düþmanlar kendi Update'lerinde bu trigger'ý görüp Chasing'e geçecek
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trigger = false;
            // Düþmanlar Chasing içindeyken bu false olunca otomatik Idle'a dönecek
        }
    }


    public void LoseFight()
    {
        //ölü diye kaydet
        gameObject.SetActive(false);
    }


    public static void RespawnAllGroupsInScene()
    {
        foreach (EnemyGroup group in GroupsInScene)
        {
            group.ResetGroup();
        }
    }
}