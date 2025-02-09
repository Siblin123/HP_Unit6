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


            if(slot.item.GetComponent<NetworkObject>().IsSpawned==true)
                csTable.Instance.gameManager.player.GetComponent<PlayerGadget>().Throw_Item_ServerRpc((ulong)slot.item.NetworkObjectId, csTable.Instance.gameManager.player.transform.position, slot.have_Count);
            else
            {
                GameObject gameObject = Instantiate(slot.item.gameObject);
                gameObject.GetComponent<NetworkObject>().Spawn();
                csTable.Instance.gameManager.player.GetComponent<PlayerGadget>().Throw_Item_ServerRpc((ulong)gameObject.GetComponent<NetworkObject>().NetworkObjectId, csTable.Instance.gameManager.player.transform.position, slot.have_Count);

            }

            slot.Update_Slot(null, 0);
            slot = null;
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Update();
        }
    }
}
