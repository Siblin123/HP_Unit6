using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
   

    // ���� ���� �� ��ȯ -> ��ġ ��ȯ
    public void Change_Slot(Inven_Slot slot1, Inven_Slot slot2)
    {
        Inven_Slot temp = new Inven_Slot();

        temp.Update_Slot(slot1.item, slot1.have_Count);
        slot1.Update_Slot(slot2.item, slot2.have_Count);
        slot2.Update_Slot(temp.item, temp.have_Count);
    }
    
    // ������ ������
    public void Throw_Slot(Inven_Slot slot)
    {
        slot.have_Count = 0;
        slot.item = null;

        slot.count_T.text = "0";
        slot.item_I.sprite = null;
    }
}
