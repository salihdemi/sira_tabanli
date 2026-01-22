using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Scriptable Objects/DataBase")]
public class DataBase : ScriptableObject
{
    public List<_Skill> skillsDataBase;
    public List<Sprite> spritesDataBase;
}


