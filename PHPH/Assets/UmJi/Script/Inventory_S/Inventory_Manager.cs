using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Mathematics;

public class Inventory_Manager : MonoBehaviour
{
    public List<Item_Info> test_L;
    public GameObject inventory;


    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(inventory.GetComponent<RectTransform>().localScale.x == 0)
            {
                inventory.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else
            {
                inventory.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            }
        }
    }

    // ������ ���� �� ����
    public bool Get_Item(Item_Info item, int count)
    {
        Player_Inventory p_I = GetComponent<Player_Inventory>();

        int i = 0;
        int j = 0;
        for (i = 0; i < p_I.slot_List.Count; i++)
        {
            // ���� �������� ������ -> �κ��丮�� �������� ������
            if (p_I.slot_List[i].item != null)
            {
                // ȹ���� �������� �̹� �κ��丮�� ������
                if (p_I.slot_List[i].item.name == item.name)
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
                        return true;
                        //break;
                    }
                }
            }
        }

        //ȹ���� �������� �κ��丮�� ���� ��
        if(i == p_I.slot_List.Count)
        {
            for (j = 0; j < p_I.slot_List.Count; j++) // ���� �־���
            {
                if (p_I.slot_List[j].item == null)
                {
                    p_I.slot_List[j].Update_Slot(item, count);
                    return true;
                    //break;
                }
            }
        }

        // �κ��丮�� �� á�� ��
        if(j == p_I.slot_List.Count)
        {
            print("�׸��Ծ� ������");
            return false;
        }
        return false;
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
            Change_Slots(slot1, slot2);
        }
        // �κ��丮 -> ���ĭ�� �����Ҷ�
        else if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
        {
            Change_Slots(slot1, slot2);
        }
        else if(slot2.item != null)
        {
            // �̵� ����
            if (slot1.item.gameObject.CompareTag(slot2.item.gameObject.tag))
            {
                Change_Slots(slot1, slot2);
            }
            else // �̵� �Ұ�
            {
                Update_Slot(slot1, slot2);
            }
        }
        // ���� ����
        else if (slot2.item == null)
        {
            // �±� ���� -> �Ϲ� �κ��丮
            if (slot2.gameObject.tag == "Untagged")
            {
                Change_Slots(slot1, slot2);
            }
            else
            {
                // �̵� ������ �����϶�
                if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
                {
                    Change_Slots(slot1, slot2);
                }
                else // �̵� �Ұ����϶�
                {
                    Update_Slot(slot1, slot2);
                }
            }
        }
    }

    private void Change_Slots(Inven_Slot slot1, Inven_Slot slot2)  // ���� ��ȯ �Լ�
    {
        Inven_Slot temp = new Inven_Slot();

        temp.Update_Slot(slot1.item, slot1.have_Count);
        slot1.Update_Slot(slot2.item, slot2.have_Count);
        slot2.Update_Slot(temp.item, temp.have_Count);
    }

    private void Update_Slot(Inven_Slot slot1, Inven_Slot slot2)
    {
        slot1.Update_Slot(slot1.item, slot1.have_Count);
        slot2.Update_Slot(slot2.item, slot2.have_Count);
    }

    // ������ ������
    public void Throw_Slot(Inven_Slot slot)
    {
        // ���� ������ �÷��̾� ��ġ�� ������

        slot.Update_Slot(null, 0); // ���� ����
    }
}
