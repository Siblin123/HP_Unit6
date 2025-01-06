using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : Inventory_Manager
{
    public static Player_Inventory instance;

    public List<Inven_Slot> slot_List;
    public List<Inven_Slot> godGet_List;

    public int money;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        print("업데이트");
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("1");
            Get_Item(test_L[0], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("2");
            Get_Item(test_L[1], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("3");
            Get_Item(test_L[2], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            print("4");
            Get_Item(test_L[3], 100);
        }
    }

}
