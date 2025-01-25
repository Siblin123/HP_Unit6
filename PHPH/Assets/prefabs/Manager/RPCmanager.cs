using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;

public class RPCmanager : NetworkBehaviour
{
    [ClientRpc]
    public bool buy_Slot_ClientRpc(bool buy_C, Item_Info item) // ������ ����
    {
        // ���� �����Ҷ�
        if (buy_C == true)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Buy_Item(item))
            {
                Shop_Manager.instance.Invent_Shop();
                return false;
            }
            else
            {
                print("�ʴ� �� �����");
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
