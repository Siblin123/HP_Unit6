using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inven_Slot : Inventory_Manager
{
    public Item_Info item; // 소지한 아이템
    public int have_Count; // 소지한 아이템 개수

    public Image item_I;
    public TextMeshProUGUI count_T;

    // 마우스 따라다니는 슬롯 관련
    public GameObject follow_Slot; // 마우스를 따라다닐 오브젝트

    private RectTransform rectTransform;
    public bool follow_C;
    public Inven_Slot clikc_S;


    private void Start()
    {
        follow_Slot = GameObject.Find("Follow_Slot"); // 카메라 따라다닐 오브젝트
        rectTransform = GetComponent<RectTransform>();


        Update_Slot(item, have_Count);

    }

    private void Update()
    {
        if (follow_C)
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform의 위치를 마우스 위치로 설정합니다.
            rectTransform.position = new Vector2(mousePosition.x + 50f, mousePosition.y + 30f);
        }
    }

    public void Update_Slot(Item_Info item, int count) // 슬롯 초기화
    {
        if (item == null) // 슬롯 초기화
        {
            this.item = null;
            have_Count = 0;
            if (count_T != null) // 장비는 개수 표시 없음
            {
                count_T.enabled = false;
            }
            if (item_I != null)
            {
                item_I.enabled = false;
            }
        }
        else
        {
            this.item = item;
            have_Count = count;

            if (count_T != null) // 장비는 개수 표시 없음
            {
                count_T.enabled = true;
                count_T.text = count.ToString();
            }
            if (item_I != null)
            {
                item_I.enabled = true;
                item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void Click_Slot() // 버튼 클릭했을때
    {
        if (follow_Slot.GetComponent<Inven_Slot>().clikc_S == null)
        {
            if(item != null) 
            {
                follow_Slot.GetComponent<Inven_Slot>().clikc_S = this;

                follow_Slot.GetComponent<Image>().enabled = true;
                follow_Slot.GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            }
        }
        else
        {
            Change_Slot(follow_Slot.GetComponent<Inven_Slot>().clikc_S, this);
            follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;

            follow_Slot.GetComponent<Image>().enabled = false;
        }
    }
}
