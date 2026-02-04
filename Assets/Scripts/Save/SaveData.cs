using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveDate;
    public string savePointName;


    public int savedScene;


    public List<AllySaveData> savedAllys = new List<AllySaveData>();

    public List<string> deadEnemyIDsInScene = new List<string>();

    // Yemeklerin isimleri ve adetleri
    public List<int> foodNumbers = new List<int>();
    public List<int> foodAmounts = new List<int>();

    // Oyuncaklarýn isimleri ve adetleri
    public List<int> toyNumbers = new List<int>();
    public List<int> toyAmounts = new List<int>();

    //Yenilen bosslar
    //Hikaye ilerlemeleri()
    //sevviye-tecrübe?

}



[System.Serializable]
public class AllySaveData
{

    public string name;
    public int sprite;

    public float currentHealth, currentStamina, currentMana;

    public float maxHealth, maxStamina, maxMana;

    public float strength, technical, focus, baseSpeed;



    public bool isDied;
    public bool isInParty;



    public int attackSkill;
    public List<int> skills = new List<int>();
}