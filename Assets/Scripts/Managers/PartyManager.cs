using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager instance;
    public BattleSpawner battleSpawner;
    private void Awake()
    {
        if (instance == null)
        {



            instance = this;

            DontDestroyOnLoad(gameObject);


            for (int i = 0; i < partyDatas.Length; i++)
            {
                UnlockAlly(partyDatas[i]);
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public List<PersistanceStats> allUnlockedAllies = new List<PersistanceStats>();
    
    [SerializeField] public CharacterData[] partyDatas; //!!!!!!!!!!gereksiz
    [SerializeField] public List<PersistanceStats> partyStats; //static olabilir



    public void OnPressRestPartyButton()
    {
        foreach (PersistanceStats stat in partyStats)
        {
            stat.GetRest();
        }
        EnemyGroup.RespawnAllGroupsInScene();
        // diðer menüyü ac
    }

    public void UnlockAlly(CharacterData data)
    {
        PersistanceStats stats = new PersistanceStats();
        stats.LoadFromBase(data);




        allUnlockedAllies.Add(stats);


        TryAddToParty(stats);//kaldirilacak

    }

    public void TryAddToParty(PersistanceStats characterStats)
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

    public void TryToRemoveFromParty(PersistanceStats characterToRemove)
    {
        if (partyStats.Count > 1 && partyStats.Contains(characterToRemove))
        {
            characterToRemove.isInParty = false;
            partyStats.Remove(characterToRemove);
        }

    }

}
