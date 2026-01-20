using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Stats/Database")]
public class GameDatabase : ScriptableObject
{
    public List<CharacterData> allCharacters;
}