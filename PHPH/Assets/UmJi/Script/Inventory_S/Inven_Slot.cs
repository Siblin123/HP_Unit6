using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inven_Slot : Inventory_Manager
{
    // 미리 인벤토리
    public bool miri_C; // 미리 인벤토리면 체크

    [Header("미리 인벤토리는 각 슬롯 번호가 있음")]
    public KeyCode slot_Count;
    // -----------------------------------------------

    [Header("잠겨있는 슬롯 사용 불가능 = false")]
    public bool unRock_C;

    public Item_Info item; // 소지한 아이템
    public int have_Count; // 소지한 아이템 개수

    public Image item_I;
    public TextMeshProUGUI count_T;


    private RectTransform rectTransform;
    public bool follow_C;
    public Inven_Slot clikc_S;

    // 상점에서 가격 미리보기
    public GameObject price_Ui;
    public GameObject price_T;

    // 더블클릭은 1초 이내에
    private float doubleC = 1f;
    public bool click_C = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Update_Slot(item, have_Count);
    }

    public override void Update()
    {

        if (follow_C) // 인벤토리 아이템 이동시 마우스를 따라다니는 이미지
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform의 위치를 마우스 위치로 설정합니다.
            rectTransform.position = new Vector2(mousePosition.x + 50f, mousePosition.y + 30f);
        }

        if (click_C) // 아이템 판매 더블클릭 체크
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

        if (miri_C == true) // 미리 인벤토리면
        {
            print(name);
            if (Input.GetKeyDown(slot_Count))
            {              
                
                                                                                            // 내 이름 -> 슬롯 번호로 변환해서 할당
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Controll(System.Convert.ToInt32(gameObject.name));
            }
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

        if (count_T != null) // 장비는 개수 표시 없음
        {
            count_T.enabled = true;
            count_T.text = have_Count.ToString("N0");
        }
    }


    public void Click_Slot() // 버튼 클릭했을때
    {
        if (unRock_C) // 잠금이 아니면
        {
            // 아무것도 클릭 안했으면 본인을 넣어줌
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().follow_Slot.GetComponent<Inven_Slot>().clikc_S == null)
            {
                if (item != null) // 아이템이 있을때
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
            // 이미 선택한게 있으면 서로 위치 교환
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

    public void Show_Price() // 가격 보여주는 함수
    {
        if (unRock_C)
        {
            Shop_Manager.instance.All_Off();

            // 아이템이 있을때만 보여줌
            if (item != null)
            {
                price_Ui.SetActive(true);
                price_Ui.GetComponent<RectTransform>().position = new Vector2(transform.position.x, transform.position.y + 100f);

                price_T.GetComponent<TextMeshProUGUI>().text = (have_Count * item.price).ToString();

                if (click_C) // 더블클릭하면 판매
                {
                    if (Sell_Item(this))
                    {
                        Shop_Manager.instance.Shop_Invent();
                        Shop_Manager.instance.Invent_Shop();
                        print("판매 성공");
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
