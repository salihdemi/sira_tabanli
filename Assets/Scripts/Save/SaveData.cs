using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveDate;
    public float playerX, playerY;//kayýt noktasý olarak deðiþtirilecek!


    public List<AllySaveData> savedAllys = new List<AllySaveData>();


    //Yenilen bosslar
    //Hikaye ilerlemeleri()
    //Kaynaklar
    //sevviye-tecrübe?

}

[System.Serializable]
public class AllySaveData
{

    public string name;
    public Sprite sprite;

    public float currentHealth;

    public float maxHealth;
    public float basePower;
    public float baseSpeed;
    public bool isDied;
    public bool isInParty;
}