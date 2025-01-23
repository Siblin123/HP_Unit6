using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Button : MonoBehaviour
{
    // 인벤토리 버튼 관련 스크립트
    public static Inven_Slot slot;

    public void Throw_Button() // 아이템 버리기
    {
     
        if(slot != null)
        {
            print("아무거나");
            // 버릴때 개수를 넣어줌
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
