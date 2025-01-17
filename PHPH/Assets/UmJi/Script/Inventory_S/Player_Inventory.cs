using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : Inventory_Manager
{
    public static Player_Inventory instance;

    /*[Header("����")]
    public GameObject shop_Panel;

    public GameObject shop_Slot_Ob; // ���� ���� �θ�
    public GameObject shop_inven_Ob; // ���� ���� �θ�*/

    [Header("�κ��丮")]
    public GameObject inven_Slot_Ob; // �κ��丮 ���� �θ�
    public GameObject godGet_Ob; // ��� ���� �θ�

    public List<Inven_Slot> slot_List;
    public List<Inven_Slot> godGet_List;

    public int money;

    private void Start()
    {
        instance = this;
        for (int i = 0; i < inven_Slot_Ob.transform.childCount; i++)
        {
            slot_List.Add(inven_Slot_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
        for (int i = 0; i < godGet_Ob.transform.childCount; i++)
        {
            godGet_List.Add(godGet_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
    }

    public override void Update()
    {

        print("������Ʈ");
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


        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.GetComponent<RectTransform>().localScale.x == 0)
            {
                inventory.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else
            {
                inventory.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            }
        }
    }



}






