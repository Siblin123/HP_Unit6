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

    // 아이템 습득 및 구매
    public bool Get_Item(Item_Info item, int count)
    {
        Player_Inventory p_I = GetComponent<Player_Inventory>();

        int i = 0;
        int j = 0;
        for (i = 0; i < p_I.slot_List.Count; i++)
        {
            // 비교할 아이템이 있으면 -> 인벤토리에 아이템이 있으면
            if (p_I.slot_List[i].item != null)
            {
                // 획득한 아이템이 이미 인벤토리에 있으면
                if (p_I.slot_List[i].item.name == item.name)
                {
                    // 총 아이템 개수가 최대 소지개수보다 많으면
                    if (p_I.slot_List[i].have_Count + count > p_I.slot_List[i].item.max_Have_Count)
                    {
                        // 일단 한계치까지 할당
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

        //획득한 아이템이 인벤토리에 없을 때
        if(i == p_I.slot_List.Count)
        {
            for (j = 0; j < p_I.slot_List.Count; j++) // 새로 넣어줌
            {
                if (p_I.slot_List[j].item == null)
                {
                    p_I.slot_List[j].Update_Slot(item, count);
                    return true;
                    //break;
                }
            }
        }

        // 인벤토리가 꽉 찼을 때
        if(j == p_I.slot_List.Count)
        {
            print("그만먹어 돼지얌");
            return false;
        }
        return false;
    }

    // 아이템 판매
    public void Sell_Item(Inven_Slot slot)
    {
        // 최대 소지 개수여야 판매가 가능하게 합니다.
        if(slot.have_Count == slot.item.max_Have_Count)
        {
            // 판매 가격
            int price = slot.item.price * slot.have_Count; 
            // 금액 증가 코드 작성

            slot.item = null;
            slot.have_Count = 0;
        }
    }

    // 슬롯 서로 값 교환 -> 위치 교환
    // slot1은 처음 클릭한거 slot2는 두번째로 클릭한거
    public void Change_Slot(Inven_Slot slot1, Inven_Slot slot2)
    {
        // 인벤토리 안에서는 장비, 아이템 상관없이 이동이 가능
        // 장비칸으로 옮길때는 각 장비칸에 맞는 장비만 착용 가능
        // 장비칸 해제 -> 빈 인벤토리로 이동

        // 인벤토리 이동일때
        if (slot1.gameObject.CompareTag(slot2.gameObject.tag))
        {
            Change_Slots(slot1, slot2);
        }
        // 인벤토리 -> 장비칸에 장착할때
        else if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
        {
            Change_Slots(slot1, slot2);
        }
        else if(slot2.item != null)
        {
            // 이동 가능
            if (slot1.item.gameObject.CompareTag(slot2.item.gameObject.tag))
            {
                Change_Slots(slot1, slot2);
            }
            else // 이동 불가
            {
                Update_Slot(slot1, slot2);
            }
        }
        // 장착 해제
        else if (slot2.item == null)
        {
            // 태그 없음 -> 일반 인벤토리
            if (slot2.gameObject.tag == "Untagged")
            {
                Change_Slots(slot1, slot2);
            }
            else
            {
                // 이동 가능한 슬롯일때
                if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
                {
                    Change_Slots(slot1, slot2);
                }
                else // 이동 불가능일때
                {
                    Update_Slot(slot1, slot2);
                }
            }
        }
    }

    private void Change_Slots(Inven_Slot slot1, Inven_Slot slot2)  // 슬롯 교환 함수
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

    // 아이템 버리기
    public void Throw_Slot(Inven_Slot slot)
    {
        // 버린 아이템 플레이어 위치에 떨어짐

        slot.Update_Slot(null, 0); // 슬롯 비우기
    }
}
