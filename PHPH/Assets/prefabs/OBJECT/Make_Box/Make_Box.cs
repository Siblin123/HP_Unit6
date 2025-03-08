using NUnit.Framework;
using System.Collections.Generic;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Make_Box : baseStatus
{
    public static Make_Box instance;

    [Header("여기부터 시작")]
    public GameObject make_View;
    public List<GameObject> make_Slot_List;

    //재료 이미지
    public List<GameObject> ingredient_List;
    public TextMeshProUGUI information_T;
    public TextMeshProUGUI item_T;
    public Image item_I;

    public override void Awake()
    {
        instance = this;

        for (int i = 0; i < make_View.transform.childCount; i++)
        {
            make_Slot_List.Add(make_View.transform.GetChild(i).gameObject);
        }
       
    }

    public void Item_In_View(Shop_Slot shop_Slot)
    {
        item_I.sprite = shop_Slot.item.gameObject.GetComponent<SpriteRenderer>().sprite;
        item_T.text = shop_Slot.item.item_Name;
        information_T.text = shop_Slot.item.explan;

        Item_Crafting_Data data = csTable.Instance.Item_Crafting_Data;
        for(int i = 0; i < ingredient_List.Count; i++)
        {
            ingredient_List[i].SetActive(false);
        }

        for (int i = 0; i < data.itemList.Count; i++) // 모든 레시피의 개수
        {
            // 레시피와 지금 내가 가지고 있는 아이템이 같을때 즉 같은 레시피일때
            if(data.itemList[i].itemName == shop_Slot.item.item_Name)
            {
                // 재료 만큼 반복
                for (int j = 0; j < data.itemList[i].materials.Count; j++)
                {
                    ingredient_List[j].SetActive(true);
                    ingredient_List[j].GetComponent<Shop_Slot>().Update_Slot(Shop_Manager.instance.find_Item(data.itemList[i].materials[j]));
                }
                break;
            }
        }
    }


    private void Update()
    {
        // 1 -> 장비 2 -> 도구 3 -> 공격 4 -> 설치 5 -> 소비
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Setting(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Setting(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Setting(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Setting(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Setting(5);
        }
    }

    public void Setting(int num)
    {
        // 초기화
        for (int i = 0; i < make_Slot_List.Count; i++)
            make_Slot_List[i].SetActive(false);

        int j = -1;
        for (int i = 0; i < make_Slot_List.Count; i++)
        {
            // 값 할당
            for (; j < csTable.Instance.Item_Crafting_Data.itemList.Count -1;)
            {
                j++;
                if (csTable.Instance.Item_Crafting_Data.itemList[j].itemType == num)
                {
                    Item_Info curItem = Shop_Manager.instance.find_Item(csTable.Instance.Item_Crafting_Data.itemList[j].itemName);
                    if (curItem != null)
                    {
                        make_Slot_List[i].SetActive(true);
                        make_Slot_List[i].GetComponent<Shop_Slot>().Update_Slot(curItem);
                        break;
                    }
                }
            }
        }
    }

    public override void interact()
    {
        base.interact();
        //상호작용을 하면 제작UI 열려야함
    }
}
