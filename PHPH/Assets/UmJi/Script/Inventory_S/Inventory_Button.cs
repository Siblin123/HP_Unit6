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
            slot.item.have_Count = slot.have_Count;
            slot.follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;
            slot.follow_Slot.GetComponent<Image>().enabled = false;

            slot.Update_Slot(null, 0);
            slot = null;
        }
    }
}
