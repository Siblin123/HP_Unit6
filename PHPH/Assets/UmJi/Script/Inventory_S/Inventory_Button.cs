using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Button : MonoBehaviour
{
    // �κ��丮 ��ư ���� ��ũ��Ʈ
    public static Inven_Slot slot;

    public void Throw_Button() // ������ ������
    {

        if (slot != null)
        {
            print("�ƹ��ų�");
            // ������ ������ �־���
            slot.item.have_Count = slot.have_Count;
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Image>().enabled = false;

            //csTable.Instance.gameManager.player.Obj_Installable(slot.item.id);


            csTable.Instance.gameManager.player.GetComponent<PlayerGadget>().Throw_Item_ServerRpc(slot.item.NetworkObjectId, csTable.Instance.gameManager.player.transform.position);

            slot.Update_Slot(null, 0);
            slot = null;
        }
    }


    
}
