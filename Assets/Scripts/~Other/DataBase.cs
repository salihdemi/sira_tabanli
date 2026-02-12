using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Scriptable Objects/DataBase")]
public class DataBase : ScriptableObject
{
    public List<Useable> useablesDataBase;
    public List<Sprite> spritesDataBase;
    public List<Weapon> weaponsDataBase;
    public List<Item> itemsDataBase;
    public List<Talisman> talismansDataBase;



}


