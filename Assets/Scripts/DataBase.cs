using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Scriptable Objects/DataBase")]
public class DataBase : ScriptableObject
{
    public static DataBase instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    public List<_Skill> skillsDataBase;
    public List<Sprite> spritesDataBase;
}
