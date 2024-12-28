using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inven_Slot : Inventory_Manager
{
    public Item_Info item; // ������ ������
    public int have_Count; // ������ ������ ����

    public Image item_I;
    public TextMeshProUGUI count_T;

    // ���콺 ����ٴϴ� ���� ����
    public GameObject follow_Slot; // ���콺�� ����ٴ� ������Ʈ

    private RectTransform rectTransform;
    public bool follow_C;
    public Inven_Slot clikc_S;


    private void Start()
    {
        follow_Slot = GameObject.Find("Follow_Slot"); // ī�޶� ����ٴ� ������Ʈ
        rectTransform = GetComponent<RectTransform>();


        Update_Slot(item, have_Count);

    }

    private void Update()
    {
        if (follow_C)
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform�� ��ġ�� ���콺 ��ġ�� �����մϴ�.
            rectTransform.position = new Vector2(mousePosition.x + 50f, mousePosition.y + 30f);
        }
    }

    public void Update_Slot(Item_Info item, int count) // ���� �ʱ�ȭ
    {
        if (item == null) // ���� �ʱ�ȭ
        {
            this.item = null;
            have_Count = 0;
            if (count_T != null) // ���� ���� ǥ�� ����
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

            if (count_T != null) // ���� ���� ǥ�� ����
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

    public void Click_Slot() // ��ư Ŭ��������
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