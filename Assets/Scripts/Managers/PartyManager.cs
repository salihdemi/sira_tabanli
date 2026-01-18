using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public BattleSpawner battleSpawner;
    private void Awake()
    {
        for (int i = 0; i < partyStats.Length; i++)
        {
            if (partyStats.Length > i && partyDatas.Length > i)
            {
                partyStats[i].LoadFromBase(partyDatas[i]);
            }
        }

        //partyProfiles = battleSpawner.SpawnAllies(party);
        //Debug.Log(party.Length);
    }
    [SerializeField] public CharacterData[] partyDatas; //!!!!!!!!!!
    [SerializeField] public PersistanceStats[] partyStats; //static olabilir



}
