using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Inventory : Inventory_Manager
{
    public static Player_Inventory instance;

    [Header("�κ��丮")]
    public GameObject inven_Slot_Ob; // �κ��丮 ���� �θ�
    public GameObject godGet_Ob; // ��� ���� �θ�

    public List<Inven_Slot> slot_List;
    public List<Inven_Slot> godGet_List;

    public GameObject follow_Slot;

    public TextMeshProUGUI money_T;
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
        if (!IsOwner)
        {
            return;
        }

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
            // �κ��丮 ����
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().inventory.GetComponent<RectTransform>().localScale.x == 0)
            {
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().inventory.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                for (int i = 0; i < slot_List.Count; i++)
                {
                    if (slot_List[i].item != null)
                    {
                        // id�� 100�� ��
                        if (slot_List[i].item.id == 100)
                        {
                            money = slot_List[i].item.have_Count;

                            money_T.text = slot_List[i].have_Count.ToString("N0");
                        }
                    }
                }
            }
            // ����
            else
            {
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().inventory.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            }
        }
    }
}






