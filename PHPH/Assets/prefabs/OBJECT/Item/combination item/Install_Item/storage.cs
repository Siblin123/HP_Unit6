using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using NPOI.OpenXmlFormats.Spreadsheet;

public class Storage : baseStatus
{
    public List<Item_Info> items = new List<Item_Info>();
    public List<Inven_Slot> slot_List = new List<Inven_Slot>(); // 인벤토리
    public List<Storage_Slot> storage_Slot = new List<Storage_Slot>(); // 창고인벤토리

    public TextMeshProUGUI money_T;
    public Inven_Slot money_Slot;
    public int money;

    public Canvas canvas;

    [Header("slot의 부모 넣어주세요 == Slot_List")]
    public GameObject slot_Parent;   
    [Header("Storage_Slot 부모 넣어주세요 == Stroge_View")]
    public GameObject storage_Slot_Parent;

    [Header("UI 캔버스")]
    public GameObject storage_Canvas;

    public override void Start()
    {
        csTable.Instance.installed_Storage.Add(this);

        for (int i = 0; i < slot_Parent.transform.childCount; i++)
        {
            slot_List.Add(slot_Parent.transform.GetChild(i).GetComponent<Inven_Slot>());
        }

        for (int i = 0; i < storage_Slot_Parent.transform.childCount; i++)
        {
            storage_Slot.Add(storage_Slot_Parent.transform.GetChild(i).GetComponent<Storage_Slot>());
        }


    

    }

    //새로 들어온 플레이어에게 창고 정보 주기
    [ServerRpc(RequireOwnership = false)]// 아이템 정보 동기화
    public void SetStorageInfo_ServerRpc()
    {
        for (int i = 0; i < storage_Slot.Count; i++)//서버의 창고 정보를 클라이언트에게 전달
        {
            if (storage_Slot[i].item!=null)
                SetStorageInfo_ClientRpc(storage_Slot[i].item.id, storage_Slot[i].have_Count,i);
            else
                SetStorageInfo_ClientRpc(-99, storage_Slot[i].have_Count, i);
        }
        print("ehdrlghk tlfgod~");
    }

    [ClientRpc]
    public void SetStorageInfo_ClientRpc(int item_Id, int item_Num, int cur_i)//서버에서 받은 창고의 정보를 
    {
        if (item_Id == -99)
        {
            storage_Slot[cur_i].Update_Slot(null, 0);
        }
        else
        {
            Item_Info curItem = Find_Item(item_Id);
            storage_Slot[cur_i].Update_Slot(curItem, item_Num);
        }
        print("ehdrlghk tjdrhd!");
    }

    //UI 활성화
    public override void interact()
    {
        base.interact();

        if (storage_Canvas.GetComponent<Canvas>().enabled == true)
        {
            storage_Canvas.GetComponent<Canvas>().enabled = false;
            Bring_Slot(1);
        }
        else
        {
            storage_Canvas.GetComponent<Canvas>().enabled = true;
            Bring_Slot(0);
        }

    }
    public void Bring_Slot(int num) // 슬롯 초기화
    {
        if (csTable.Instance.gameManager.player == null)
            return;

        if(num == 0) // 인벤토리 값 가져오기
        {
            for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
            {
                slot_List[i].Update_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].item, csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].have_Count);
            }

            Money_Slot_Find();
            money_T.text = money.ToString();
        }
        else // 인벤토리에 값 할당하기
        {
            for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
            {
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].Update_Slot(slot_List[i].item, slot_List[i].have_Count);
            }

            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Money_Slot_Find();
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Update();
        }
    }
    public void Money_Slot_Find()
    {
        int i = 0;
        for (i = 0; i < slot_List.Count; i++)
        {
            if (slot_List[i].item != null)
            {
                // id가 100은 돈
                if (slot_List[i].item.id == 0)
                {
                    money_Slot = slot_List[i];

                    money = slot_List[i].have_Count;

                    break;
                }
            }
        }

        if (i == slot_List.Count)
        {
            money_Slot = null;

            money = 0;
        }
        money_T.text = money.ToString("N0");
    }

    [ServerRpc(RequireOwnership = false)]// 아이템 넣는거
    public void AddItemServerRpc(int id)
    {
        if (!csTable.Instance.gameManager.player.IsServer)
            return;
        items.Add(Find_Item(id));
        SyncItemsClientRpc(id);
        Update_ItemUI();
    }

    [ClientRpc]
    private void SyncItemsClientRpc(int id)
    {
        if (csTable.Instance.gameManager.player.IsServer)
            return;

        items.Add(Find_Item(id));
        Update_ItemUI();
    }


    // 아이템 제거
    [ServerRpc(RequireOwnership = false)]
    public void RemoveItemServerRpc(int id)
    {
        if (!csTable.Instance.gameManager.player.IsServer)
            return;
        items.Remove(Find_Item(id));
        RemoveItemClientRpc(id);
        Update_ItemUI();
    }

    [ClientRpc]
    public void RemoveItemClientRpc(int id)
    {
        if (csTable.Instance.gameManager.player.IsServer)
            return;
        items.Remove(Find_Item(id));
        Update_ItemUI();
    }

    public void Update_ItemUI()
    {


    }

    public Item_Info Find_Item(int id)
    {
        foreach (var item in csTable.Instance.allItem_List)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }

     public void Update()
    {
        if (storage_Canvas.GetComponent<Canvas>().enabled == true)
        {
            //상자와 거리가 멀어지면 UI 비활성화 || ESC 누르면 UI 비활성화
            if (Vector2.Distance(transform.position, csTable.Instance.gameManager.player.transform.position) > 2 ||
                Input.GetKeyDown(KeyCode.Escape))
            {
                storage_Canvas.GetComponent<Canvas>().enabled = false;
            }
        }


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetStorageInfo_ServerRpc();
        }

    }
}
