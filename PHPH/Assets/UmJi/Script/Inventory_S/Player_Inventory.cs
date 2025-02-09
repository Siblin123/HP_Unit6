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

    [Header("인벤토리")]
    public GameObject inven_Slot_Ob; // 인벤토리 슬롯 부모
    public GameObject godGet_Ob; // 장비 슬롯 부모
    public GameObject miri_Ob; // 미리보기 인벤토리 부모
    public GameObject miri_Panel; // 미리보기 판넬

    public int unRock_SlotCount = 6; // 잠금 해제인 인벤토리 개수

    public List<Inven_Slot> slot_List;
    public List<Inven_Slot> godGet_List;
    public List<Inven_Slot> miri_List;

    public GameObject follow_Slot;

    public TextMeshProUGUI money_T;
    public int money; // 플레이어 소지금

    public Inven_Slot money_Slot;
    public GameObject money_Ob;

    // Miri_Inven_Controll() // 미리인벤토리 관련
    public int currentSlot = 0; // 현재 선택된 슬롯 (1~6)
    public GameObject frame_Ob; // 선택한 슬롯 표시

    private int maxSlots = 5; // 총 슬롯 개수

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

    public override void Get_Item(Item_Info item, int count) // 재정의
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

    public bool Buy_Item(Item_Info item, ulong playerId, ulong slotId) // 아이템 구매
    {
        if (!IsOwner)
        {
            return false;
        }

        if (playerId != NetworkObjectId)
            return false;

        // 소지금액이 구매할 아이템의 금액보다 많으면
        if (Shop_Manager.instance.money >= item.max_Have_Count * item.price)
        {
            if (Get_Item_OK(item, item.max_Have_Count)) // 인벤토리에 아이템을 넣 을 수 있는지 확인
            {
                money -= item.max_Have_Count * item.price;
                money_T.text = money.ToString();
                money_Slot.Update_Slot(money_Slot.item, money);
                Slot_Rock_ServerRpc(slotId, playerId);
                return true;
            }
            else // 인벤토리에 칸 없음
            {
                return false;
            }
        }
        else // 돈 부족
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
                if (networkObject_.GetComponent<NetworkObject>().IsOwnedByServer)//생성은 서버 에서만
                {
                    Item_Info item = null;

                    item = slot.item;

                    GameObject slotItem = Instantiate(item.gameObject);

                    slotItem.GetComponent<NetworkObject>().Spawn();
                    Get_Item(slotItem.GetComponent<Item_Info>(), item.max_Have_Count); //-> 실제로 인벤토리에 아이템 할당 해주는 함수
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
        // -> 이 호출은 미리 슬롯에서 불러오는 거임
        if(num != 7){
            currentSlot = num;
            frame_Ob.transform.position = miri_List[currentSlot].gameObject.transform.position;

            csTable.Instance.gameManager.player.arm_Anim._anim = ArmType.empty_P;
        }
        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f) // 휠을 위로 올릴 때 (왼쪽으로 이동)
            {
                currentSlot--;
                if (currentSlot < 0) currentSlot = maxSlots;
            }
            else if (scroll < 0f) // 휠을 아래로 내릴 때 (오른쪽으로 이동)
            {
                currentSlot++;
                if (currentSlot > maxSlots) currentSlot = 0;
            }

            // 스크롤을 움직이고 있을 때
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
            // 인벤토리 켜줌
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().inventory.GetComponent<RectTransform>().localScale.x == 0)
            {
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().inventory.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Money_Slot_Find();
            }
            // 꺼줌
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
                // id가 100은 돈
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