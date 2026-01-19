using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public BattleSpawner battleSpawner;
    private void Awake()
    {
        for (int i = 0; i < partyDatas.Length; i++)
        {
            UnlockAlly(partyDatas[i]);
        }

    }

    public List<PersistanceStats> allUnlockedAllys = new List<PersistanceStats>();
    
    [SerializeField] public CharacterData[] partyDatas; //!!!!!!!!!!gereksiz
    [SerializeField] public List<PersistanceStats> partyStats; //static olabilir

    
    public void UnlockAlly(CharacterData data)
    {
        PersistanceStats stats = new PersistanceStats();
        stats.LoadFromBase(data);




        allUnlockedAllys.Add(stats);


        Debug.Log("added" + stats.originData);

        TryAddToParty(stats);
    }

    private void TryAddToParty(PersistanceStats characterStats)
    {
        if (partyStats.Count < 4)
        {
            partyStats.Add(characterStats);
        }
        else
        {
            Debug.Log("parti dolu");
        }
    }

    private void SetParty(PersistanceStats characterToAdd, PersistanceStats characterToRemove)
    {
        // characterToRemove gerçekten partide mi kontrolü
        int index = partyStats.IndexOf(characterToRemove);

        if (index != -1)
        {
            // Direkt o index'teki karakteri yenisiyle deðiþtir (Swap)
            partyStats[index] = characterToAdd;
            Debug.Log("Karakter deðiþtirildi.");
        }
    }

}
