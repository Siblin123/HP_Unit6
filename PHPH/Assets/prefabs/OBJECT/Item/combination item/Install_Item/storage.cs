using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;

public class Storage : Item_Info
{
    public List<Item_Info> items = new List<Item_Info>();

    [ServerRpc(RequireOwnership = false)]
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
