using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    // ������ ���� �� ����
    public void Get_Item(Item_Info item, int count)
    {
        Player_Inventory p_I = GetComponent<Player_Inventory>();

        for (int i = 0; i < p_I.slot_List.Count; i++)
        {
            // ȹ���� �������� �κ��丮�� ������
            if (p_I.slot_List[i].item == item)
            {
                // �� ������ ������ �ִ� ������������ ������
                if (p_I.slot_List[i].have_Count + count > p_I.slot_List[i].item.max_Have_Count)
                {
                    for (int j = 0; j < p_I.slot_List.Count; i++)
                    {
                        if(p_I.slot_List[j].item == null)
                        {
                            // num => �ִ� ���� ���� - ���� ��������
                            int num = p_I.slot_List[i].have_Count + count;
                            num = num - p_I.slot_List[i].item.max_Have_Count;

                            // �κ��丮�� �ִ� ���� �� �������� ������ �Ҵ�
                            p_I.slot_List[j].Update_Slot(p_I.slot_List[i].item, num);
                            p_I.slot_List[i].have_Count = p_I.slot_List[i].item.max_Have_Count;
                            break;
                        }
                    }
                    break;
                }
                else { p_I.slot_List[i].have_Count += count; break; }
            }
        }
        /* for(int i = 0;  i < Player_Inventory.instance.slot_List.Count; i++)
         {
             // ȹ���� �������� �κ��丮�� ������
             if(Player_Inventory.instance.slot_List[i].item == item)
             {
                 // �� ������ ������ �ִ� ������������ ������
                 if(Player_Inventory.instance.slot_List[i].have_Count + count > Player_Inventory.instance.slot_List[i].item.max_Have_Count)
                 {
                     for(int j = 0; j < Player_Inventory.instance.slot_List.Count; i++)
                     {

                     }
                     // num => �ִ� ���� ���� - ���� ��������
                     int num = Player_Inventory.instance.slot_List[i].have_Count + count;
                     num = num - Player_Inventory.instance.slot_List[i].item.max_Have_Count;


                     Player_Inventory.instance.slot_List[i].have_Count = Player_Inventory.instance.slot_List[i].item.max_Have_Count;

                 }
                 else { Player_Inventory.instance.slot_List[i].have_Count += count; }
             }
         }*/
    }

    // ������ �Ǹ�
    public void Sell_Item(Inven_Slot slot)
    {
        // �ִ� ���� �������� �ǸŰ� �����ϰ� �մϴ�.
        if(slot.have_Count == slot.item.max_Have_Count)
        {
            // �Ǹ� ����
            int price = slot.item.price * slot.have_Count; 
            // �ݾ� ���� �ڵ� �ۼ�

            slot.item = null;
            slot.have_Count = 0;
        }
    }

    // ���� ���� �� ��ȯ -> ��ġ ��ȯ
    // slot1�� ó�� Ŭ���Ѱ� slot2�� �ι�°�� Ŭ���Ѱ�
    public void Change_Slot(Inven_Slot slot1, Inven_Slot slot2)
    {
        // �κ��丮 �ȿ����� ���, ������ ������� �̵��� ����
        // ���ĭ���� �ű涧�� �� ���ĭ�� �´� ��� ���� ����
        // ���ĭ ���� -> �� �κ��丮�� �̵�

        // �κ��丮 �̵��϶�
        if (slot1.gameObject.CompareTag(slot2.gameObject.tag))
        {
            Update_Slots(slot1, slot2);
        }
        // �κ��丮 -> ���ĭ�� �����Ҷ�
        else if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
        {
            Update_Slots(slot1, slot2);
        }
        // ���� ����
        else if (slot2.item == null)
        {
            Update_Slots(slot1, slot2);
        }
    }

    private void Update_Slots(Inven_Slot slot1, Inven_Slot slot2)
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
