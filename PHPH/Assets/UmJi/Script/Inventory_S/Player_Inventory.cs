using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using static UnityEditor.Progress;
using static Arm_Anim;
using NUnit.Framework.Interfaces;

public class Player_Inventory : Inventory_Manager
{
    public static Player_Inventory instance;

    [Header("�κ��丮")]
    public GameObject inven_Slot_Ob; // �κ��丮 ���� �θ�
    public GameObject godGet_Ob; // ��� ���� �θ�
    public GameObject miri_Ob; // �̸����� �κ��丮 �θ�
    public GameObject miri_Panel; // �̸����� �ǳ�

    public int unRock_SlotCount = 6; // ��� ������ �κ��丮 ����

    public List<Inven_Slot> slot_List;
    public List<Inven_Slot> godGet_List;
    public List<Inven_Slot> miri_List;

    public GameObject follow_Slot;

    public TextMeshProUGUI money_T;
    public int money; // �÷��̾� ������

    public Inven_Slot money_Slot;
    public GameObject money_Ob;

    // Miri_Inven_Controll() // �̸��κ��丮 ����
    public int currentSlot = 0; // ���� ���õ� ���� (1~6)
    public GameObject frame_Ob; // ������ ���� ǥ��

    private int maxSlots = 5; // �� ���� ����

    private void Start()
    {
        if (!IsOwner)
        {
            return;
        }
        instance = this;
        miri_Panel.SetActive(true);

        for (int i = 0; i < inven_Slot_Ob.transform.childCount; i++)
        {
            slot_List.Add(inven_Slot_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
        for (int i = 0; i < godGet_Ob.transform.childCount; i++)
        {
            godGet_List.Add(godGet_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
        for (int i = 0; i < miri_Ob.transform.childCount; i++)
        {
            miri_List.Add(miri_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
    }

    public override void Get_Item(Item_Info item, int count) // ������
    {
        base.Get_Item(item, count);
        Miri_Inven_Update();
    }
    
    public void Miri_Inven_Update()
    {
        for (int i = 0; i < miri_List.Count; i++)
        {
            miri_List[i].Update_Slot(slot_List[i].item, slot_List[i].have_Count);
        }
    }

    public bool Buy_Item(Item_Info item, ulong playerId, ulong slotId) // ������ ����
    {
        if (!IsOwner)
        {
            return false;
        }

        if (playerId != NetworkObjectId)
            return false;

        // �����ݾ��� ������ �������� �ݾ׺��� ������
        if (Shop_Manager.instance.money >= item.max_Have_Count * item.price)
        {
            if (Get_Item_OK(item, item.max_Have_Count)) // �κ��丮�� �������� �� �� �� �ִ��� Ȯ��
            {
                money -= item.max_Have_Count * item.price;
                money_T.text = money.ToString();
                money_Slot.Update_Slot(money_Slot.item, money);
                Slot_Rock_ServerRpc(slotId, playerId);
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
    public void Slot_Rock_ServerRpc(ulong slotId, ulong playerId)
    {
        Slot_Rock_ClientRpc(slotId, playerId);
    }
    [ClientRpc]
    public void Slot_Rock_ClientRpc(ulong slotId, ulong playerId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(slotId, out NetworkObject networkObject))
        {
            Shop_Slot slot = networkObject.GetComponent<Shop_Slot>();
            slot.buy_C = true;


           
            if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerId, out NetworkObject networkObject_))
            {
                if (networkObject_.GetComponent<NetworkObject>().IsOwnedByServer)//������ ���� ������
                {
                    Item_Info item = null;

                    item = slot.item;

                    GameObject slotItem = Instantiate(item.gameObject);

                    slotItem.GetComponent<NetworkObject>().Spawn();
                    Get_Item(slotItem.GetComponent<Item_Info>(), item.max_Have_Count); //-> ������ �κ��丮�� ������ �Ҵ� ���ִ� �Լ�
                }
                else
                {
                    Spawn_Item_ServerRpc(slot.item.id, playerId);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void Spawn_Item_ServerRpc(int id, ulong playerID)
    {
        foreach(Item_Info item in csTable.Instance.allItem_List)
        {
            if(id== item.id)
            {
                GameObject item_ = Instantiate(item.gameObject);

                item_.GetComponent<NetworkObject>().Spawn();

                if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerID, out NetworkObject networkObject))
                {
                    Spawn_Item_ClientRpc(item_.GetComponent<NetworkObject>().NetworkObjectId, playerID);
                    break;
                }

            }
        }
    }

    [ClientRpc]
    public void Spawn_Item_ClientRpc(ulong itemid, ulong playerID)
    {
        if (csTable.Instance.gameManager.player.NetworkObjectId != playerID)
            return;

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(playerID, out NetworkObject networkObject))
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(itemid, out NetworkObject item))
            { 
                networkObject.GetComponent<Player_Inventory>().Get_Item(item.GetComponent<Item_Info>(), item.GetComponent<Item_Info>().max_Have_Count);

            }
        }
    }
    public override void Update()
    {
        base.Update();

        if (!IsOwner)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("1");
            Get_Item(test_L[0], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            print("2");
            Get_Item(test_L[1], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            print("3");
            Get_Item(test_L[2], 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            print("4");
            Get_Item(test_L[3], 100);
        }


        Inventory_On_Off();
        Miri_Inven_Controll();
    }

    public void Miri_Inven_Controll(int num = 7)
    {
        // -> �� ȣ���� �̸� ���Կ��� �ҷ����� ����
        if(num != 7){
            currentSlot = num;
            frame_Ob.transform.position = miri_List[currentSlot].gameObject.transform.position;

            csTable.Instance.gameManager.player.arm_Anim._anim = ArmType.empty_P;
        }
        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f) // ���� ���� �ø� �� (�������� �̵�)
            {
                currentSlot--;
                if (currentSlot < 0) currentSlot = maxSlots;
            }
            else if (scroll < 0f) // ���� �Ʒ��� ���� �� (���������� �̵�)
            {
                currentSlot++;
                if (currentSlot > maxSlots) currentSlot = 0;
            }

            // ��ũ���� �����̰� ���� ��
            if (scroll != 0)
            {
                frame_Ob.transform.position = miri_List[currentSlot].gameObject.transform.position;
                csTable.Instance.gameManager.player.arm_Anim._anim = ArmType.empty_P;
            }
        }

        if (miri_List[currentSlot].item == null) { csTable.Instance.gameManager.player.GetComponent<PlayerGadget>().curItem = null; }
        else { csTable.Instance.gameManager.player.GetComponent<PlayerGadget>().curItem = miri_List[currentSlot].item; }
    }

    public void Inventory_On_Off()
    {
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