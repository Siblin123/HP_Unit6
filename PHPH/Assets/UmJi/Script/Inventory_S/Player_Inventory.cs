using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

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
    public int money; // �÷��̾� ������

    public Inven_Slot money_Slot;
    public GameObject money_Ob;

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

    public bool Buy_Item(Item_Info item,ulong playerId, ulong slotId) // ������ ����
    {
        if (playerId != NetworkObjectId)
            return false;

        // �����ݾ��� ������ �������� �ݾ׺��� ������
        if (Shop_Manager.instance.money >= item.max_Have_Count * item.price)
        {
            if(Get_Item(item, item.max_Have_Count))
            {
                money -= item.max_Have_Count * item.price;
                money_T.text = money.ToString();
                money_Slot.Update_Slot(money_Slot.item, money);
                Slot_Rock_ServerRpc(slotId);
                return true;
            }
            else // �κ��丮�� ĭ ����
            {
                return false;
            }
        }
        else // �� ����
        {
            return false;
        }
    }


    [ServerRpc]
    public void Slot_Rock_ServerRpc(ulong slotId)
    {
        Slot_Rock_ClientRpc(slotId);
    }
    [ClientRpc]
    public void Slot_Rock_ClientRpc(ulong slotId)
    {
        print("ASDASDASDASDAASD");
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(slotId, out NetworkObject networkObject))
        {
           Shop_Slot slot = networkObject.GetComponent<Shop_Slot>();
            slot.buy_C = true;
        }
    }

    public override void Update()
    {
        if (!IsOwner)
        {
            return;
        }

     
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
                Money_Slot_Find();
            }
            // ����
            else
            {
               // money_Slot = null;

                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().inventory.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            }
        }
    }

    public void Money_Slot_Find()
    {
        for (int i = 0; i < slot_List.Count; i++)
        {
            if (slot_List[i].item != null)
            {
                // id�� 100�� ��
                if (slot_List[i].item.id == 0)
                {
                    money_Slot = slot_List[i];

                    money = slot_List[i].have_Count;

                    money_T.text = slot_List[i].have_Count.ToString("N0");
                }
            }
        }
    }
}






