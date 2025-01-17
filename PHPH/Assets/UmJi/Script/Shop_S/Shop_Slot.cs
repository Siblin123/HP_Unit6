using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Shop_Slot : Shop_Manager
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

    public void buy_Slot() // ������ ����
    {
         if(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Get_Item(item, item.max_Have_Count))
         {
            // �κ��丮 -> ���� �κ��丮 ����ȭ
            for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
            {
                inven_Slot_List[i].Update_Slot(Player_Inventory.instance.slot_List[i].item, csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].have_Count);

                // id�� 100�� ��
                if (inven_Slot_List[i].item.id == 100)
                {
                    money = inven_Slot_List[i].item.have_Count;

                    money_View.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inven_Slot_List[i].have_Count.ToString("N0");
                }
            }
            print("����");
         }
        
    }
}
