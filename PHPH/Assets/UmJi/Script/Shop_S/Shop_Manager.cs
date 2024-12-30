using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Shop_Manager : interaction
{
    public GameObject shop_Panel;

    public List<Shop_Slot> slot_List; // 상점 슬롯 리스트

    [Header("기본 아이템 리스트")]
    public List<Item_Info> base_Item_List;
    [Header("완성 아이템 리스트")]
    public List<Item_Info> combination_Item_List;
    [Header("모든 아이템 리스트")]
    public List<Item_Info> all_Item_List;

    public List<Inven_Slot> inven_Slot_List; // 인벤토리 슬롯

    private void Start()
    {
        Update_Slot();
    }

    public override void interact()
    {
        base.interact();
        On_Off();
    }

    private void OnEnable()
    {
        inven_Slot_List = new List<Inven_Slot>();

        for (int i = 0; i < Player_Inventory.instance.slot_List.Count; i++)
        {
            inven_Slot_List.Add(Player_Inventory.instance.slot_List[i]);
            inven_Slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
        }
    }

    public void On_Off()
    {
        if (shop_Panel.activeSelf == true)
        {
            shop_Panel.SetActive(false);
        }
        else
        {
            shop_Panel.SetActive(true);
            inven_Slot_List = Player_Inventory.instance.slot_List;
            for (int i = 0; i < inven_Slot_List.Count; i++) 
            {
                inven_Slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
            }
        }
    }

    public Item_Info find_Item(string name)
    {
        for(int i = 0; i < all_Item_List.Count; i++)
        {
            if (all_Item_List[i].name == name)
            {
                return all_Item_List[i];
            }
        }
        return null;
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
                select_N = CheckDuplicate(base_Item_List, select_N);
                slot_List[i].Update_Slot(base_Item_List[select_N]);
                base_N++;
            }
            else
            {
                slot_List[i].Update_Slot(all_Item_List[select_N]);
                select_N = CheckDuplicate(all_Item_List, select_N);
            }
        }
    }

    public int CheckDuplicate(List<Item_Info> list, int select_N) // 중복 체크
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
