using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Storage_Slot : Inven_Slot
{
    public override void Click_Slot()
    {
        if(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S == null)
        {
            if (item != null)
            {
                if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Get_Item_OK(item, have_Count))
                {
                    
                   
                    csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Get_Item(item, have_Count);
                   
                    transform.parent.root.GetComponent<Storage>().Bring_Slot(0);
                    RemoveItemServerRpc(item.id);
                }
            }
        }
        else
        {
            base.Click_Slot();

            AddItemServerRpc(item.id, have_Count);
            transform.parent.root.GetComponent<Storage>().Bring_Slot(1);
        }

        csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Update();
        transform.parent.root.GetComponent<Storage>().Money_Slot_Find();
        money_T.text = transform.parent.root.GetComponent<Storage>().money.ToString();
    }



    // -------------------------- <<동기화 구간>> -----------------------------
    [ServerRpc(RequireOwnership = false)]// 아이템 넣는거
    public void AddItemServerRpc(int id, int have_Count)
    {
        if (!csTable.Instance.gameManager.player.IsServer)
            return;
        
        SyncItemsClientRpc(id, have_Count);
        Update_ItemUI();
    }

    [ClientRpc]
    private void SyncItemsClientRpc(int id, int have_Count)
    {
        Update_Slot(Find_Item(id), have_Count);
    }


    // 아이템 제거
    [ServerRpc(RequireOwnership = false)]
    public void RemoveItemServerRpc(int id)
    {
        if (!csTable.Instance.gameManager.player.IsServer)
            return;
        
        RemoveItemClientRpc(id);
       
    }

    [ClientRpc]
    public void RemoveItemClientRpc(int id)
    {

        Update_Slot(null, 0); // 

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

}
