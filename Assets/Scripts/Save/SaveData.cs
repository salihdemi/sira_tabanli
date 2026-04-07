using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string saveDate;
    public string savePointID;


    public int savedScene;


    public List<AllySaveData> savedAllys = new List<AllySaveData>();

    public List<string> deadEnemyIDsInScene = new List<string>();
    public List<string> collectedIDs = new List<string>();
    public List<NpcDialogSaveData> npcDialogIndexes = new List<NpcDialogSaveData>();

    // Yemeklerin isimleri ve adetleri
    public InventorySaveData inventorySaveData;


    //Yenilen bosslar
    //Hikaye ilerlemeleri()
    //sevviye-tecr�be?

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
    public string weapon;
    public string item;
    public string talisman;


    public bool isDied;
    public bool isInParty;


    public string sprite;
    public string attackSkill;
    public List<string> unlockedSkills = new List<string>();
    public List<string> currentSkills = new List<string>();

}
[System.Serializable]
public class NpcDialogSaveData
{
    public string npcID;
    public int dialogIndex;
}
[System.Serializable]
public class InventorySaveData
{
    // Dictionary i�in: �simler ve Adetler
    public List<string> consumableNames = new List<string>();
    public List<int> consumableAmounts = new List<int>();

    // Listeler ve HashSetler i�in: Sadece isimler
    public List<string> ownedWeaponNames = new List<string>();
    public List<string> equippedWeaponNames = new List<string>();

    public List<string> ownedTalismanNames = new List<string>();
    public List<string> equippedTalismanNames = new List<string>();

    public List<string> ownedItemNames = new List<string>();
    public List<string> equippedItemNames = new List<string>();
}