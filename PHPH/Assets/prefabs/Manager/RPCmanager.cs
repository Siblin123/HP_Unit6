using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;

public class RPCmanager : NetworkBehaviour
{



    [ClientRpc]
    public void buy_Slot_ClientRpc(ulong id) // 아이템 구매
    {
        Shop_Slot slot=null;

        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out NetworkObject networkObject))
        {
            slot= networkObject.GetComponent<Shop_Slot>();

        }



        print("asdasdasdasd");
        // 구매 가능할때
        if (slot.buy_C == false)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Buy_Item(slot.item))
            {
                Shop_Manager.instance.Invent_Shop();
                slot. buy_C = true;
            }
            else
            {
                print("너님 돈 없어요");
            }
        }
    }
}
