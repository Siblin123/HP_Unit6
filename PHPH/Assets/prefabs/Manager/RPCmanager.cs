using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;

public class RPCmanager : NetworkBehaviour
{
    public bool buy_Slot(bool buy_C, Item_Info item) // ������ ����
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
                return true;
                print("�ʴ� �� �����");
            }
        }
        else
        {
            return false;
        }
    }
}
