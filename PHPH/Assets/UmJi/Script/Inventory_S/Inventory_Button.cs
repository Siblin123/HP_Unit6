using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Button : MonoBehaviour
{
    // 인벤토리 버튼 관련 스크립트
    public static Inven_Slot slot;

    public void Throw_Button() // 아이템 버리기
    {

        if (slot != null)
        {
            print("아무거나");
            // 버릴때 개수를 넣어줌
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
