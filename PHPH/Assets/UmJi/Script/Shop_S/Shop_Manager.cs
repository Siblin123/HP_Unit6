using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.Netcode;
using static UnityEditor.Progress;

public class Shop_Manager : interaction
{
    public static Shop_Manager instance;

    public GameObject shop_Panel;

    public GameObject shop_Slot_Ob; // 상점 슬롯 부모
    public GameObject inven_Slot_Ob; // 상점 인벤토리

    public List<Shop_Slot> slot_List; // 상점 슬롯 리스트
    public List<Inven_Slot> inven_Slot_List; // 인벤토리 슬롯

    [Header("기본 아이템 리스트")]
    public List<Item_Info> base_Item_List;
    [Header("완성 아이템 리스트")]
    public List<Item_Info> combination_Item_List;
    [Header("모든 아이템 리스트")]
    public List<Item_Info> all_Item_List;

    // 돈 표시 UI
    public Inven_Slot money_Slot;
    public TextMeshProUGUI money_T;
    public int money;

    // 전부 꺼줄 이미지
    public GameObject price_Ui;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < shop_Slot_Ob.transform.childCount; i++)
        {
            slot_List.Add(shop_Slot_Ob.transform.GetChild(i).GetComponent<Shop_Slot>());
        }
        for (int i = 0; i < inven_Slot_Ob.transform.childCount; i++)
        {
            inven_Slot_List.Add(inven_Slot_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
    }

    public override void interact()
    {
        base.interact();
        On_Off();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // 상점 온오프
        {
            On_Off();
        }
        else if (Input.GetKeyDown(KeyCode.K)) // 상점 판매 아이템 리롤
        {
            Update_Slot();
        }
    }
    public void All_Off()
    {
        price_Ui.SetActive(false);
    }

    public void On_Off() // 상점 끄고 켜기
    {
        // 인벤토리 끄기
        if (shop_Panel.activeSelf == true)
        {
            // 상점 인벤토리 -> 인벤토리에 적용
            Shop_Invent();

            money_Slot = null;
            shop_Panel.SetActive(false);
        }
        else if (shop_Panel.activeSelf == false) // 켜기
        {
            shop_Panel.SetActive(true);

            // 인벤토리 -> 상점 인벤토리에 적용
            Invent_Shop();
        }
    }

    public void Shop_Invent()
    {
        // 상점 인벤토리 -> 인벤토리에 적용
        for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
        {
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Money_Slot_Find();

            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);     
        }
    }

    public void Invent_Shop()
    {
        // 인벤토리 -> 상점 인벤토리에 적용
        for (int i = 0; i < inven_Slot_List.Count; i++)
        {
            inven_Slot_List[i].Update_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].item, csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].have_Count);

            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Money_Slot_Find();
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

    public void Update_Slot() // 판매할 아이템 표시
    {
        // 기본 이아팀, 완성 아이템
        int base_N = 0, combi_N = 0;

        int select_N = 0;

        for (int i = 0; i < slot_List.Count; i++)
        {
            if (combi_N < 2) // 각 두개씩 나옴
            {
                select_N = CheckDuplicate(combination_Item_List, select_N);
                slot_List[i].Update_Slot(combination_Item_List[select_N]);
                Update_Slot_ClientRpc(combination_Item_List[select_N].id, i);
                combi_N++;
            }
            else if (base_N < 2)
            {
                select_N = CheckDuplicate(base_Item_List, select_N);
                slot_List[i].Update_Slot(base_Item_List[select_N]);
                Update_Slot_ClientRpc(base_Item_List[select_N].id, i);
                base_N++;
            }
            else
            {
                select_N = CheckDuplicate(all_Item_List, select_N);
                slot_List[i].Update_Slot(all_Item_List[select_N]);
                Update_Slot_ClientRpc(all_Item_List[select_N].id, i);
            }
        }
    }

    [ClientRpc]
    void Update_Slot_ClientRpc(int id, int index) // 아이템 서버, 클라이언트 동기화
    {
        if (IsServer)
            return;

        print("클라만");


        for (int j = 0; j < all_Item_List.Count; j++)
        {
            if (all_Item_List[j].id == id)
            {
                GameObject.Find("Shop_Manager").GetComponent<Shop_Manager>().slot_List[index].Update_Slot(all_Item_List[j]);
                break;
            }
        }
    }

    public int CheckDuplicate(List<Item_Info> list, int select_N) // 중복 체크
    {
        int checkDuplicate = Random.Range(0, list.Count);

        // slot_List에 같은 아이템이 있는지 확인
        bool isDuplicate = false;

        foreach (var slot in slot_List)
        {
            if (slot.item != null)
            {
                if (slot.item.id == list[checkDuplicate].id)
                {
                    isDuplicate = true; // 중복 발견
                    break;
                }
            }
            else
            {
                isDuplicate = false;
                break;
            }
        }

        // 중복이 없으면 반복 종료
        if (isDuplicate == false)
        {
            return checkDuplicate; // 선택된 아이템 반환
        }
        else
        {
           return CheckDuplicate(list, select_N);
        }
    }
}
