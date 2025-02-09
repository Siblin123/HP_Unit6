using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inven_Slot : Inventory_Manager
{
    // �̸� �κ��丮
    public bool miri_C; // �̸� �κ��丮�� üũ

    [Header("�̸� �κ��丮�� �� ���� ��ȣ�� ����")]
    public KeyCode slot_Count;
    // -----------------------------------------------

    [Header("����ִ� ���� ��� �Ұ��� = false")]
    public bool unRock_C;

    public Item_Info item; // ������ ������
    public int have_Count; // ������ ������ ����

    public Image item_I;
    public TextMeshProUGUI count_T;


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
        rectTransform = GetComponent<RectTransform>();

        Update_Slot(item, have_Count);
    }

    public override void Update()
    {

        if (follow_C) // �κ��丮 ������ �̵��� ���콺�� ����ٴϴ� �̹���
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform�� ��ġ�� ���콺 ��ġ�� �����մϴ�.
            rectTransform.position = new Vector2(mousePosition.x + 50f, mousePosition.y + 30f);
        }

        if (click_C) // ������ �Ǹ� ����Ŭ�� üũ
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

        if (miri_C == true) // �̸� �κ��丮��
        {
            print(name);
            if (Input.GetKeyDown(slot_Count))
            {              
                
                                                                                            // �� �̸� -> ���� ��ȣ�� ��ȯ�ؼ� �Ҵ�
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Controll(System.Convert.ToInt32(gameObject.name));
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
        if (unRock_C) // ����� �ƴϸ�
        {
            // �ƹ��͵� Ŭ�� �������� ������ �־���
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S == null)
            {
                if (item != null) // �������� ������
                {
                    Inventory_Button.slot = this;

                    csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S = this;

                    csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Image>().enabled = true;
                    csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;

                    if(item_I!=null && count_T!=null)
                    {
                        item_I.enabled = false;
                        count_T.enabled = false;
                    }
                   
                }
            }
            // �̹� �����Ѱ� ������ ���� ��ġ ��ȯ
            else
            {
                Change_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S, this);
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S = null;

                Inventory_Button.slot = null;

                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Image>().enabled = false;
            }

            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Money_Slot_Find();
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Update();
            
        }
    }

    public void Show_Price() // ���� �����ִ� �Լ�
    {
        if (unRock_C)
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
                        Shop_Manager.instance.Shop_Invent();
                        Shop_Manager.instance.Invent_Shop();
                        print("�Ǹ� ����");
                    }
                }
                else
                {
                    click_C = true;
                }
            }
        }
    }
}
