using UnityEngine;

public class PartyManager : MonoBehaviour
{

    private void Awake()
    {
        for (int i = 0; i < party.Length; i++)
        {
            if (party.Length > i && datas.Length > i)
            {
                party[i].LoadFromBase(datas[i]);
            }
        }
    }
    [SerializeField] public CharacterData[] datas; //!!!!!!!!!!
    [SerializeField] public PersistanceStats[] party; //static olabilir
}
