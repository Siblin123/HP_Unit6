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
            slot.item.have_Count = slot.have_Count;
            slot.follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;
            slot.follow_Slot.GetComponent<Image>().enabled = false;

            slot.Update_Slot(null, 0);
            slot = null;
        }
    }
}
