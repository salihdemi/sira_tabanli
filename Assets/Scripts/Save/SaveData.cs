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
    public List<int> consumableNumbers = new List<int>();
    public List<int> consumableAmounts = new List<int>();


    //Yenilen bosslar
    //Hikaye ilerlemeleri()
    //sevviye-tecrübe?

}



[System.Serializable]
public class AllySaveData
{

    public string name;
    public string weaponType;

    public float currentHealth, currentStamina, currentMana;

    public float maxHealth, maxStamina, maxMana;

    public float strength, technical, focus, baseSpeed;

    //weapon-item-charm!
    public int weapon;
    public int item;
    public int charm;


    public bool isDied;
    public bool isInParty;


    public int sprite;
    public int attackSkill;
    public List<int> skills = new List<int>();

}
[System.Serializable]
public class InventorySaveData
{
    // Dictionary için: Ýsimler ve Adetler
    public List<string> consumableNames = new List<string>();
    public List<int> consumableAmounts = new List<int>();

    // Listeler ve HashSetler için: Sadece isimler
    public List<string> ownedWeaponNames = new List<string>();
    public List<string> equippedWeaponNames = new List<string>();

    public List<string> ownedTalismanNames = new List<string>();
    public List<string> equippedTalismanNames = new List<string>();

    public List<string> ownedItemNames = new List<string>();
    public List<string> equippedItemNames = new List<string>();
}