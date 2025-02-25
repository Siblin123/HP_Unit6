using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game Data/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item_Crafting_Data.ItemData> items = new List<Item_Crafting_Data.ItemData>();
}