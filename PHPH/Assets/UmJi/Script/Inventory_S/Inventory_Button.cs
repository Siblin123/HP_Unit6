using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Button : MonoBehaviour
{
    // �κ��丮 ��ư ���� ��ũ��Ʈ
    public static Inven_Slot slot;

    public void Throw_Button() // ������ ������
    {
     
        if(slot != null)
        {
            print("�ƹ��ų�");
            // ������ ������ �־���
            slot.item.have_Count = slot.have_Count;
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Image>().enabled = false;

            csTable.Instance.gameManager.player.Obj_Installable(slot.item.id);
            /*if (csTable.Instance.gameManager.player.IsClient)
            {
                Throw_Item_ServerRpc();
            }
            else if (csTable.Instance.gameManager.player.IsServer)
            {
                GameObject clone = Instantiate(slot.item.gameObject, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
                clone.GetComponent<NetworkObject>().Spawn();
            }*/

            slot.Update_Slot(null, 0);
            slot = null;
        }
    }

    [ServerRpc]
    public void Throw_Item_ServerRpc()
    {
        if (csTable.Instance.gameManager.player.IsClient)
        {
            return;
        }

        GameObject clone = Instantiate(slot.item.gameObject, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
        clone.GetComponent<NetworkObject>().Spawn();
    }
}
