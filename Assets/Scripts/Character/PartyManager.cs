using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public static class PartyManager
{

    public static List<PersistanceStats> allUnlockedAllies = new List<PersistanceStats>();
    
    public static List<PersistanceStats> partyStats = new List<PersistanceStats>();


    public static PartyVisualizer visualizer;

    public static void OnPressRestPartyButton()
    {
        foreach (PersistanceStats stat in partyStats)
        {
            stat.Regen();
        }
        EnemyGroup.RespawnAllGroupsInScene();
        // diðer menüyü ac
    }

    public static void UnlockAlly(CharacterData data)
    {
        PersistanceStats stats = new PersistanceStats();
        stats.LoadFromBase(data);




        allUnlockedAllies.Add(stats);


        TryAddToParty(stats);//kaldirilacak?

        if(visualizer == null)
        {
            GameObject go = new GameObject("PartyVisualizer");
            visualizer = go.AddComponent<PartyVisualizer>();
            visualizer.partyStats = partyStats;
        }
    }

    public static void TryAddToParty(PersistanceStats characterStats)
    {
        if (partyStats.Count < 4)
        {
            characterStats.isInParty = true;
            partyStats.Add(characterStats);
        }
        else
        {
            Debug.Log("parti dolu");
        }
    }

    public static void TryToRemoveFromParty(PersistanceStats characterToRemove)
    {
        if (partyStats.Count > 1 && partyStats.Contains(characterToRemove))
        {
            characterToRemove.isInParty = false;
            partyStats.Remove(characterToRemove);
        }

    }

}
public class PartyVisualizer : MonoBehaviour
{
    public List<PersistanceStats> partyStats = new List<PersistanceStats>();
}
