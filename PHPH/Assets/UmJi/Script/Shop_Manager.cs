using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Shop_Manager : MonoBehaviour
{
    public GameObject shop_Ob;
    public List<Shop_Slot> slot_List; // ���� ���� ����Ʈ
    public bool on_Off_C = false;

    [Header("�⺻ ������ ����Ʈ")]
    public List<Item_Info> base_Item_List;
    [Header("�ϼ� ������ ����Ʈ")]
    public List<Item_Info> combination_Item_List;
    [Header("��� ������ ����Ʈ")]
    public List<Item_Info> all_Item_List;

    private void Start()
    {
        Update_Slot();
    }
    public void On_Off()
    {
        if (on_Off_C)
        {
            shop_Ob.SetActive(false);
        }
        else
        {
            shop_Ob.SetActive(true);
        }
    }

    public void Update_Slot()
    {
        int base_N = 0, combi_N = 0;

        int select_N = 0;
        int test;
        for (int i = 0; i < slot_List.Count; i++)
        {
            if (combi_N < 2)
            {
                select_N = CheckDuplicate(combination_Item_List, select_N);
                slot_List[i].Update_Slot(combination_Item_List[select_N]);
                combi_N++;
            }
            else if (base_N < 2)
            {
                /*select_N = Random.Range(0, combination_Item_List.Count);
                slot_List[i].Update_Slot(combination_Item_List[select_N]);*/
                select_N = CheckDuplicate(base_Item_List, select_N);
                slot_List[i].Update_Slot(base_Item_List[select_N]);
                base_N++;
            }
            else
            {
                /* select_N = Random.Range(0, all_Item_List.Count);
                 slot_List[i].Update_Slot(all_Item_List[select_N]);*/

                slot_List[i].Update_Slot(all_Item_List[select_N]);
                select_N = CheckDuplicate(all_Item_List, select_N);
            }
        }
    }

    public int CheckDuplicate(List<Item_Info> list, int select_N) // �ߺ� üũ
    {
        int checkDuplicate = Random.Range(0, list.Count);
        while (true)
        {
            if (checkDuplicate != select_N)
            {
                return checkDuplicate;
            }
            else
            {
                checkDuplicate = Random.Range(0, list.Count);
            }
        }
    }
}
