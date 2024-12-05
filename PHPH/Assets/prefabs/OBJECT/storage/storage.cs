using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Collections.Generic;

public class Storage : NetworkBehaviour
{
    public List<Image> item_UI; 

    public List<Item> all_Item;
    public List<string> items = new List<string>();

    [ServerRpc(RequireOwnership = false)]
    public void AddItemServerRpc(string item)
    {
        if (!IsServer)
            return;
        items.Add(item);
        SyncItemsClientRpc(item); 
        Update_ItemUI();
    }

    [ClientRpc]
    private void SyncItemsClientRpc(string item)
    {
        if(IsServer)
            return;    

        items.Add(item);
        Update_ItemUI();
    }


    // ������ ����
    [ServerRpc(RequireOwnership = false)]
    public void RemoveItemServerRpc(string item)
    {
        if (!IsServer)
            return;
        items.Remove(item);
        RemoveItemClientRpc(item); 
        Update_ItemUI();
    }

    [ClientRpc]
    public void RemoveItemClientRpc(string item)
    {
        if (IsServer)
            return;
        items.Remove(item);
        Update_ItemUI();
    }

    public void Update_ItemUI()
    {

       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(IsServer)
            {
                print("S");
                AddItemServerRpc(all_Item[0].itemName); // �������� ������ �߰�
            }
            
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(!IsServer)
            {
                print("C");
                AddItemServerRpc(all_Item[1].itemName); // �������� ������ �߰�
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (IsServer)
            {
                print("SS");
                RemoveItemServerRpc(all_Item[0].itemName); // �������� ������ ����
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (!IsServer)
            {
                print("CC");
                RemoveItemServerRpc(all_Item[1].itemName); // �������� ������ ����
            }

        }



    }
}
