using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.Profiling;

public class FightPanelObjectPool : MonoBehaviour
{
    //sistem disable ederken veriyi temizlemedi, enable edince uzerine yaziyor!
    public static FightPanelObjectPool instance;



    [Header("Parents")]
    [SerializeField] private Transform allyParent;
    [SerializeField] private Transform enemyParent;

    [Header("Prefabs")] // gerekirse yeni prefab uretmek icin

    [SerializeField] private GameObject allyPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Settings")]
    [SerializeField] private int initialPoolSize = 4;

    // Uyuyan objeleri tutan listeler
    private List<AllyProfileLungeHandler> allyPool = new List<AllyProfileLungeHandler>();
    private List<EnemyProfileLungeHandler> enemyPool = new List<EnemyProfileLungeHandler>();





    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // Oyun baþýnda küçük bir hazýrlýk (Pre-warm)
            //PreparePool();

            Profile.OnSomeoneDie += HandleReturnToPool;
            gameObject.SetActive(false);
        }
        else Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneDie -= HandleReturnToPool;
    }
    private void HandleReturnToPool(Profile profile)
    {
        if (profile is Profile ally)
        {
            ReturnAllyToPool(ally);
        }
        else if (profile is Profile enemy)
        {
            ReturnEnemyToPool(enemy);
        }
    }


    private void PreparePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAlly();
            CreateNewEnemy();
        }
    }


    #region --- ALLY ÝÞLEMLERÝ ---
    public AllyProfileLungeHandler GetAlly()
    {
        foreach (AllyProfileLungeHandler ally in allyPool)//aktif olmayaný bul ona yaz
        {
            if (!ally.gameObject.activeInHierarchy)
            {
                return ally;
            }
        }
        return CreateNewAlly(); // Yeni yaratýlanýn içinde de SetActive(true) olmalý
    }
    public void ReturnAllyToPool(Profile ally)
    {
        // Objenin üzerindeki tüm geçici verileri temizle
        //ally.lungeHandler.ClearSkillAndTarget();
        //ally.ResetStats();

        // Obje artýk "uyuyan" statüsüne geçer
        ally.gameObject.SetActive(false);
    }
    private AllyProfileLungeHandler CreateNewAlly()
    {
        GameObject obj = Instantiate(allyPrefab, allyParent);
        AllyProfileLungeHandler lungeHandler = obj.GetComponent<AllyProfileLungeHandler>();
        allyPool.Add(lungeHandler);
        return lungeHandler;
    }
    public void ClearAllies()
    {
        List<Profile> activeAllies = FightManager.AllyProfiles;

        for (int i = activeAllies.Count - 1; i >= 0; i--)
        {
            ReturnAllyToPool(activeAllies[i]);
        }
    }
    #endregion


    #region --- ENEMY ÝÞLEMLERÝ ---
    public EnemyProfileLungeHandler GetEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                return enemy;
            }
        }
        return CreateNewEnemy();
    }
    public void ReturnEnemyToPool(Profile enemy)
    {
        //enemy.lungeHandler.ClearSkillAndTarget();
        //enemy.ResetStats();

        enemy.gameObject.SetActive(false);
    }
    private EnemyProfileLungeHandler CreateNewEnemy()
    {
        GameObject obj = Instantiate(enemyPrefab, enemyParent);
        EnemyProfileLungeHandler lungeHandler = obj.GetComponent<EnemyProfileLungeHandler>();
        enemyPool.Add(lungeHandler);
        return lungeHandler;
    }
    public void ClearEnemies()
    {
        List<Profile> activeEnemies = FightManager.EnemyProfiles;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            ReturnEnemyToPool(activeEnemies[i]);
        }
    }
    #endregion
}
