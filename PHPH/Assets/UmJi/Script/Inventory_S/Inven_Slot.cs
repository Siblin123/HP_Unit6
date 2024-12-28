using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inven_Slot : MonoBehaviour
{
    public Item_Info item; // 소지한 아이템
    public int have_Count; // 소지한 아이템 개수

    public Image item_I;
    public TextMeshProUGUI count_T;
    public GameObject follow_Slot; // 마우스 따라다니는 슬롯

    private RectTransform rectTransform;
    public bool follow_C;

    private void Start()
    {
        follow_Slot = GameObject.Find("Follow_Slot"); // 카메라 따라다닐 오브젝트
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (follow_C)
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform의 위치를 마우스 위치로 설정합니다.
            rectTransform.position = mousePosition;
        }
    }

    public void Update_Slot(Item_Info item, int count) // 슬롯 초기화
    {
        this.item = item;
        have_Count = count;
       
        if (count_T != null) // 장비는 개수 표시 없음
        {
            count_T.text = count.ToString();
        }

        item_I.enabled = true;
        item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void Click_Slot() // 버튼 클릭했을때
    {
        follow_Slot.GetComponent<Inven_Slot>().Update_Slot(item, have_Count);
    }
}
