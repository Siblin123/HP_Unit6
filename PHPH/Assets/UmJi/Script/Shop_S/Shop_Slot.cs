using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class Shop_Slot : NetworkBehaviour
{
    //구매했는지 
    public bool buy_C; 

    public Item_Info item;
    public TextMeshProUGUI price_T;
    public TextMeshProUGUI name_T;
    public TextMeshProUGUI count_T;
    public Image item_I;
    public int item_Count;
    private int price;

    public void Update_Slot(Item_Info item, int count = 0)
    {
        if(item == null)
        {
            this.item = null;
            item_Count = 0;
            price = 0;
        }
        else
        {
            this.item = item;
            item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            name_T.text = item.item_Name.ToString();

            if (count_T != null)
            {
                item_Count = item.max_Have_Count;
                count_T.text = item.max_Have_Count.ToString();
            }
            if (price_T != null)
            {
                price = item_Count * item.price;
                price_T.text = price.ToString();
            }
            if (count != 0)
            {
                item_Count = count;
                count_T.text = item_Count.ToString();
            }
        }
    }
 

    public void buy_Slot() // 아이템 구매
    {
        // 구매 가능할때
        if (buy_C == false)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Buy_Item(item, csTable.Instance.gameManager.player.NetworkObjectId , this.NetworkObjectId))
            {
               
                buy_C = true;
            }
            else
            {
                print("너님 돈 없어요");
            }
        }
    }
}
