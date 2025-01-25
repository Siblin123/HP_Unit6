using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using Unity.Netcode;

public class Shop_Slot : NetworkBehaviour
{
    //구매했는지 
    public bool buy_C = false; 

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

    [ClientRpc]
    public void buy_Slot_ClientRpc() // 아이템 구매
    {
        // 구매 가능할때
        if (buy_C == false)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Buy_Item(item))
            {
                Shop_Manager.instance.Invent_Shop();
                buy_C = true;
            }
            else
            {
                print("너님 돈 없어요");
            }
        }
    }
}
