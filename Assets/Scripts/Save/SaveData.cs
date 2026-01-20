using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string saveDate;
    public float playerX, playerY;

    // Açýlan karakterlerin isim listesi
    public List<AllySaveData> savedAllys = new List<AllySaveData>();

    // O anki aktif partideki karakterlerin isim listesi
    public List<string> currentPartyIDs = new List<string>();
}

[System.Serializable]
public class AllySaveData
{
    public string characterID; // ScriptableObject'in dosya adý (name)
}