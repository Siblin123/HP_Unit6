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

    // �������� ���� �̸�����
    public GameObject price_Ui;
    public GameObject price_T;

    // ����Ŭ���� 1�� �̳���
    private float doubleC = 1f;
    public bool click_C = false;

    private void Start()
    {
        follow_Slot = GameObject.Find("Follow_Slot"); // ī�޶� ����ٴ� ������Ʈ
        rectTransform = GetComponent<RectTransform>();

        Update_Slot(item, have_Count);
    }

    public override void Update()
    {
        base.Update();
        if (follow_C)
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform�� ��ġ�� ���콺 ��ġ�� �����մϴ�.
            rectTransform.position = new Vector2(mousePosition.x + 50f, mousePosition.y + 30f);
        }

        if (click_C)
        {
            if(doubleC <= 0)
            {
                doubleC = 1f;
                click_C = false;
            }
            else
            {
                doubleC -= Time.deltaTime;
            }
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
                count_T.text = count.ToString("N0");
            }
            if (item_I != null)
            {
                item_I.enabled = true;
                item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void Pluse_Item(int num)
    {
        have_Count += num;

        if (count_T != null) // ���� ���� ǥ�� ����
        {
            count_T.enabled = true;
            count_T.text = have_Count.ToString("N0");
        }
    }

    public void Click_Slot() // ��ư Ŭ��������
    {
        // �ƹ��͵� Ŭ�� �������� ������ �־���
        if (follow_Slot.GetComponent<Inven_Slot>().clikc_S == null) 
        {
            if(item != null) // �������� ������
            {
                Inventory_Button.slot = this;

                follow_Slot.GetComponent<Inven_Slot>().clikc_S = this;

                follow_Slot.GetComponent<Image>().enabled = true;
                follow_Slot.GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;

                item_I.enabled = false;
                count_T.enabled = false;
            }
        }
        // �̹� �����Ѱ� ������ ���� ��ġ ��ȯ
        else
        {
            Change_Slot(follow_Slot.GetComponent<Inven_Slot>().clikc_S, this);
            follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;
            Inventory_Button.slot = null;

            follow_Slot.GetComponent<Image>().enabled = false;
        }
    }

    public void Show_Price() // ���� �����ִ� �Լ�
    {
        Shop_Manager.instance.All_Off();

        // �������� �������� ������
        if (item != null)
        {
            price_Ui.SetActive(true);
            price_Ui.GetComponent<RectTransform>().position = new Vector2(transform.position.x, transform.position.y + 100f);

            price_T.GetComponent<TextMeshProUGUI>().text = (have_Count * item.price).ToString();

            if (click_C) // ����Ŭ���ϸ� �Ǹ�
            {
                if (Sell_Item(this))
                {
                    print("�Ǹ� ����");
                }
                else
                {
                    print("�Ǹ� �Ұ���");
                }
            }
            else
            {
                click_C = true;
            }
        }
    }
}
