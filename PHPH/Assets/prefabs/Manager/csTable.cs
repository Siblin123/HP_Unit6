using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class csTable : MonoBehaviour
{
    public static csTable Instance;
    public GameManager gameManager;
    public Transform unitSopn_Pos;
    public Shop_Manager shop_Manager;
    public NetworkSpawnManager NetworkSpawnManager; 


    public List<Item_Info> allItem_List;
    [HideInInspector]
    public Player_Inventory Player_Inventory;
    private void Awake()
    {
        Instance = this;

    }
}
