using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;
using TMPro;

public class Storage : Item_Info
{
    public List<Item_Info> items = new List<Item_Info>();
    public List<Inven_Slot> slot_List = new List<Inven_Slot>(); // 인벤토리

    public TextMeshProUGUI money_T;
    public Inven_Slot money_Slot;
    public int money;

    private void OnEnable()
    {
        Bring_Slot(0);
    }

    private void OnDisable()
    {
        Bring_Slot(1);
    }

    public void Bring_Slot(int num) // 슬롯 초기화
    {
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

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsServer)
            {
                print("S");
                AddItemServerRpc(1); 
            }
            if (!IsServer)
            {
                print("C");
                AddItemServerRpc(2); 
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (IsServer)
            {
                print("SS");
                RemoveItemServerRpc(1); 
            }
            if (!IsServer)
            {
                print("CC");
                RemoveItemServerRpc(2);
            }
        }

    }
}
