using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Inventory_Manager : MonoBehaviour
{
    public List<Item_Info> test_L;

    // ������ ���� �� ����
    public void Get_Item(Item_Info item, int count)
    {
        Player_Inventory p_I = GetComponent<Player_Inventory>();

        int i = 0;
        for (i = 0; i < p_I.slot_List.Count; i++)
        {
            // ȹ���� �������� �κ��丮�� ������
            if (p_I.slot_List[i].item == item)
            //if (p_I.slot_List[i].id == item.id)
            {
                // �� ������ ������ �ִ� ������������ ������
                if (p_I.slot_List[i].have_Count + count > p_I.slot_List[i].item.max_Have_Count)
                {
                    // �ϴ� �Ѱ�ġ���� �Ҵ�
                    p_I.slot_List[i].Update_Slot(p_I.slot_List[i].item, p_I.slot_List[i].item.max_Have_Count);

                    count += p_I.slot_List[i].have_Count;
                    count -= p_I.slot_List[i].item.max_Have_Count;
                }
                else 
                {
                    p_I.slot_List[i].Pluse_Item(count);
                    break; 
                }
            }
        }
        //ȹ���� �������� �κ��丮�� ���� ��
        if(i == p_I.slot_List.Count)
        {
            for (int j = 0; j < p_I.slot_List.Count; j++)
            {
                if (p_I.slot_List[j].item == null)
                {
                    p_I.slot_List[j].Update_Slot(item, count);
                    break;
                }
            }
        }
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
        else if(slot2.item != null)
        {
            if (slot1.item.gameObject.CompareTag(slot2.item.gameObject.tag))
            {
                Update_Slots(slot1, slot2);
            }
        }
        // ���� ����
        else if (slot2.item == null)
        {
            // �±� ���� -> �Ϲ� �κ��丮
            if (slot2.gameObject.tag == "Untagged")
            {
                Update_Slots(slot1, slot2);
            }
            else
            {
                if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
                {
                    Update_Slots(slot1, slot2);
                }
            }
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
