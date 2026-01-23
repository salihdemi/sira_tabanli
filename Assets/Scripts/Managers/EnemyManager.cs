using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public List<EnemyGroup> allNormalGroups = new List<EnemyGroup>();

    EnemyManager()
    {
        instance = this;
    }

    // "DÝNLEN" butonuna basýnca bu fonksiyon çaðrýlacak
    public void RespawnAll()
    {
        foreach (var group in allNormalGroups)
        {
            group.ResetGroup();
        }
    }
}