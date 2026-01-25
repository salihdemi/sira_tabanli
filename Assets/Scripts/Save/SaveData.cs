using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveDate;
    public float playerX, playerY;


    public int savedScene;


    public List<AllySaveData> savedAllys = new List<AllySaveData>();

    public List<bool> savedEnemyGroups = new List<bool>();

    public List<string> deadEnemyIDs = new List<string>();

    //Yenilen bosslar
    //Hikaye ilerlemeleri()
    //Kaynaklar
    //sevviye-tecrübe?

}



[System.Serializable]
public class AllySaveData
{

    public string name;
    public int sprite;

    public float currentHealth;

    public float maxHealth;
    public float basePower;
    public float baseSpeed;
    public bool isDied;
    public bool isInParty;



    public int attackSkill;
    public List<int> skills;
}