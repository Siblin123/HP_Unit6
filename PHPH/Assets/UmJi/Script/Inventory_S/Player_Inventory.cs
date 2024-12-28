using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : Inventory_Manager
{
    public static Player_Inventory instance;

    public List<Inven_Slot> slot_List;
    public List<Inven_Slot> godGet_List;

    private void Awake()
    {
        instance = this;
    }
}
