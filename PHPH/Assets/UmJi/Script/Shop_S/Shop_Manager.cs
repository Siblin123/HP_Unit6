using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Shop_Manager : interaction
{
    public GameObject shop_Panel;

    public GameObject shop_Slot_Ob; // 상점 슬롯 부모
    public GameObject shop_inven_Ob; // 상점 슬롯 부모

    public List<Shop_Slot> slot_List; // 상점 슬롯 리스트
    public List<Inven_Slot> inven_Slot_List; // 인벤토리 슬롯

    [Header("기본 아이템 리스트")]
    public List<Item_Info> base_Item_List;
    [Header("완성 아이템 리스트")]
    public List<Item_Info> combination_Item_List;
    [Header("모든 아이템 리스트")]
    public List<Item_Info> all_Item_List;

    // 돈 표시 UI
    public GameObject money_View;



    private void Start()
    {
        Update_Slot();
        for (int i = 0; i < shop_Slot_Ob.transform.childCount; i++)
        {
            slot_List.Add(shop_Slot_Ob.transform.GetChild(i).GetComponent<Shop_Slot>());
        }
        for (int i = 0; i < shop_inven_Ob.transform.childCount; i++)
        {
            inven_Slot_List.Add(shop_inven_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
    }

    public override void interact()
    {
        base.interact();
        On_Off();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            On_Off();
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < Player_Inventory.instance.slot_List.Count; i++)
        {
            inven_Slot_List[i].Update_Slot(Player_Inventory.instance.slot_List[i].item, Player_Inventory.instance.slot_List[i].have_Count);

            // id가 100은 돈
            if (inven_Slot_List[i].item.id == 100)
            {
                money_View.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inven_Slot_List[i].have_Count.ToString("N0");
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < Player_Inventory.instance.slot_List.Count; i++)
        {
            Player_Inventory.instance.slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
        }
    }

    public void On_Off() // 상점 끄고 켜기
    {
        // 인벤토리 끄기
        if (shop_Panel.activeSelf == true)
        {
            // 상점에서 판매 또는 구매한걸 인벤토리에 적용
            for (int i = 0; i < Player_Inventory.instance.slot_List.Count; i++)
            {
                Player_Inventory.instance.slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
            }

            shop_Panel.SetActive(false);
        }
        else if (shop_Panel.activeSelf == false) // 켜기
        {
            shop_Panel.SetActive(true);

            // 인벤토리를 상점 인벤토리에 적용
            for (int i = 0; i < inven_Slot_List.Count; i++)
            {
                inven_Slot_List[i].Update_Slot(Player_Inventory.instance.slot_List[i].item, Player_Inventory.instance.slot_List[i].have_Count);
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
