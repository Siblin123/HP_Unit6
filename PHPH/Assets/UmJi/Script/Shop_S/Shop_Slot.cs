using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Shop_Slot : MonoBehaviour
{
    public Item_Info item;
    public TextMeshProUGUI price_T;
    public TextMeshProUGUI name_T;
    public TextMeshProUGUI count_T;
    public Image item_I;
    private int item_Count;
    private int price;

    public void Update_Slot(Item_Info item)
    {
        this.item = item;
        item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
        name_T.text = item.name.ToString();

        item_Count = item.max_Have_Count;
        count_T.text = item.max_Have_Count.ToString();

        price = item_Count * item.price;
        price_T.text = price.ToString();
    }

    public void buy_Slot() // 아이템 구매
    {
         if(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Get_Item(item, item.max_Have_Count))
         {
            // 인벤토리 -> 상점 인벤토리 동기화
            for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
            {
                Shop_Manager.instance.inven_Slot_List[i].Update_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].item, csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].have_Count);

                if(Shop_Manager.instance.inven_Slot_List[i].item != null)
                {
                    // id가 100은 돈
                    if (Shop_Manager.instance.inven_Slot_List[i].item.id == 100)
                    {
                        Shop_Manager.instance.money = Shop_Manager.instance.inven_Slot_List[i].item.have_Count;

                        Shop_Manager.instance.money_T.text = Shop_Manager.instance.inven_Slot_List[i].have_Count.ToString("N0");
                    }
                }
            }
            print("구매");
         }
        
    }
}
