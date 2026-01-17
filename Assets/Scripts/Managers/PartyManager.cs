using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public BattleSpawner battleSpawner;
    private void Awake()
    {
        for (int i = 0; i < party.Length; i++)
        {
            if (party.Length > i && datas.Length > i)
            {
                party[i].LoadFromBase(datas[i]);
            }
        }

        partyProfiles = battleSpawner.SpawnAllies(party);
        Debug.Log(party.Length);
    }
    [SerializeField] public CharacterData[] datas; //!!!!!!!!!!
    [SerializeField] public PersistanceStats[] party; //static olabilir
    [SerializeField] public List <AllyProfile> partyProfiles = new List<AllyProfile>();

    public void SaveHealths()
    {
        foreach (AllyProfile partyProfile in partyProfiles)
        {
            partyProfile.SaveHealth();
        }
        //pratyProfiles = AllyProfile[]
    }
}
