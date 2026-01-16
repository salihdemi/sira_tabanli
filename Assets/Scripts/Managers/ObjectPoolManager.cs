using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    //sistem disable ederken veriyi temizlemedi, enable edince uzerine yaziyor!

    [SerializeField] private FightManager fightManager;

    [Header("Parents")]
    [SerializeField] private Transform allyParent;
    [SerializeField] private Transform enemyParent;

    [Header("Prefabs")] // gerekirse yeni prefab uretmek icin

    [SerializeField] private GameObject allyPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Settings")]
    [SerializeField] private int initialPoolSize = 4;

    // Uyuyan objeleri tutan listeler
    private List<AllyProfile> allyPool = new List<AllyProfile>();
    private List<EnemyProfile> enemyPool = new List<EnemyProfile>();

    private void Awake()
    {
        // Oyun baþýnda küçük bir hazýrlýk (Pre-warm)
        PreparePool();

        Profile.OnSomeoneDie += HandleReturnToPool;
    }
    private void OnDestroy()
    {
        Profile.OnSomeoneDie -= HandleReturnToPool;
    }
    private void HandleReturnToPool(Profile profile)
    {
        if (profile is AllyProfile ally)
        {
            ReturnAllyToPool(ally);
        }
        else if (profile is EnemyProfile enemy)
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
    public AllyProfile GetAlly()
    {
        foreach (var ally in allyPool)
        {
            if (!ally.gameObject.activeInHierarchy)
            {
                ally.gameObject.SetActive(true);
                return ally;
            }
        }
        return CreateNewAlly(); // Yeni yaratýlanýn içinde de SetActive(true) olmalý
    }
    public void ReturnAllyToPool(AllyProfile ally)
    {
        // Objenin üzerindeki tüm geçici verileri temizle
        ally.ClearSkillAndTarget();
        //ally.ResetStats();

        // Obje artýk "uyuyan" statüsüne geçer
        ally.gameObject.SetActive(false);
    }
    private AllyProfile CreateNewAlly()
    {
        GameObject obj = Instantiate(allyPrefab, allyParent);
        AllyProfile profile = obj.GetComponent<AllyProfile>();
        obj.SetActive(false);//! denemek gerek
        allyPool.Add(profile);
        return profile;
    }
    public void ClearAllies()
    {
        List<AllyProfile> activeAllies = fightManager.turnScheduler.ActiveAllyProfiles;

        for (int i = activeAllies.Count - 1; i >= 0; i--)
        {
            ReturnAllyToPool(activeAllies[i]);
        }
    }
    #endregion


    #region --- ENEMY ÝÞLEMLERÝ ---
    public EnemyProfile GetEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                enemy.gameObject.SetActive(true);
                return enemy;
            }
        }
        return CreateNewEnemy();
    }
    public void ReturnEnemyToPool(EnemyProfile enemy)
    {
        enemy.ClearSkillAndTarget();
        //enemy.ResetStats();

        enemy.gameObject.SetActive(false);
    }
    private EnemyProfile CreateNewEnemy()
    {
        GameObject obj = Instantiate(enemyPrefab, enemyParent);
        EnemyProfile profile = obj.GetComponent<EnemyProfile>();
        obj.SetActive(false);//! denemek gerek
        enemyPool.Add(profile);
        return profile;
    }
    public void ClearEnemies()
    {
        List<EnemyProfile> activeEnemies = fightManager.turnScheduler.ActiveEnemyProfiles;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            ReturnEnemyToPool(activeEnemies[i]);
        }
    }
    #endregion
}
