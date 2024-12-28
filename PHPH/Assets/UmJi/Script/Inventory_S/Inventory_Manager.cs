using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    // 아이템 습득 및 구매
    public void Get_Item(Item_Info item, int count)
    {
        Player_Inventory p_I = GetComponent<Player_Inventory>();

        for (int i = 0; i < p_I.slot_List.Count; i++)
        {
            // 획득한 아이템이 인벤토리에 있으면
            if (p_I.slot_List[i].item == item)
            {
                // 총 아이템 개수가 최대 소지개수보다 많으면
                if (p_I.slot_List[i].have_Count + count > p_I.slot_List[i].item.max_Have_Count)
                {
                    for (int j = 0; j < p_I.slot_List.Count; i++)
                    {
                        if(p_I.slot_List[j].item == null)
                        {
                            // num => 최대 소지 개수 - 현재 보유개수
                            int num = p_I.slot_List[i].have_Count + count;
                            num = num - p_I.slot_List[i].item.max_Have_Count;

                            // 인벤토리에 최대 값을 뺀 나머지지 개수를 할당
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
             // 획득한 아이템이 인벤토리에 있으면
             if(Player_Inventory.instance.slot_List[i].item == item)
             {
                 // 총 아이템 개수가 최대 소지개수보다 많으면
                 if(Player_Inventory.instance.slot_List[i].have_Count + count > Player_Inventory.instance.slot_List[i].item.max_Have_Count)
                 {
                     for(int j = 0; j < Player_Inventory.instance.slot_List.Count; i++)
                     {

                     }
                     // num => 최대 소지 개수 - 현재 보유개수
                     int num = Player_Inventory.instance.slot_List[i].have_Count + count;
                     num = num - Player_Inventory.instance.slot_List[i].item.max_Have_Count;


                     Player_Inventory.instance.slot_List[i].have_Count = Player_Inventory.instance.slot_List[i].item.max_Have_Count;

                 }
                 else { Player_Inventory.instance.slot_List[i].have_Count += count; }
             }
         }*/
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
            Update_Slots(slot1, slot2);
        }
        // 인벤토리 -> 장비칸에 장착할때
        else if (slot1.item.gameObject.CompareTag(slot2.gameObject.tag))
        {
            Update_Slots(slot1, slot2);
        }
        // 장착 해제
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

    // 아이템 버리기
    public void Throw_Slot(Inven_Slot slot)
    {
        slot.have_Count = 0;
        slot.item = null;

        slot.count_T.text = "0";
        slot.item_I.sprite = null;
    }
}
