using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;

public class RPCmanager : NetworkBehaviour
{
    public bool buy_Slot(bool buy_C, Item_Info item) // 아이템 구매
    {
        // 구매 가능할때
        if (buy_C == true)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Buy_Item(item))
            {
                Shop_Manager.instance.Invent_Shop();
                return false;
            }
            else
            {
                return true;
                print("너님 돈 없어요");
            }
        }
        else
        {
            return false;
        }
    }
}
