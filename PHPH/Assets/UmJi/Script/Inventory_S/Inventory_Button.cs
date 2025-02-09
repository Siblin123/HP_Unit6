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
