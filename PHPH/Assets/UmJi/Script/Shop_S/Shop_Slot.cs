using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Shop_Slot : MonoBehaviour
{
    public bool buy_C; //구매했는지 

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
        // 구매 가능할때
        if(buy_C == true)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Buy_Item(item))
            {
                Shop_Manager.instance.Invent_Shop();
                buy_C = false;
            }
            else
            {
                print("너님 돈 없어요");
            }
        }
    }
}
