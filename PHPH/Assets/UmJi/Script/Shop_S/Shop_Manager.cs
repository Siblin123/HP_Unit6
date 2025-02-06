using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.Netcode;
using static UnityEditor.Progress;

public class Shop_Manager : interaction
{
    public static Shop_Manager instance;

    public GameObject shop_Panel;

    public GameObject shop_Slot_Ob; // ���� ���� �θ�
    public GameObject inven_Slot_Ob; // ���� �κ��丮

    public List<Shop_Slot> slot_List; // ���� ���� ����Ʈ
    public List<Inven_Slot> inven_Slot_List; // �κ��丮 ����

    [Header("�⺻ ������ ����Ʈ")]
    public List<Item_Info> base_Item_List;
    [Header("�ϼ� ������ ����Ʈ")]
    public List<Item_Info> combination_Item_List;
    [Header("��� ������ ����Ʈ")]
    public List<Item_Info> all_Item_List;

    // �� ǥ�� UI
    public Inven_Slot money_Slot;
    public TextMeshProUGUI money_T;
    public int money;

    //public GameObject bar_Reset;

    // ���� ���� �̹���
    public GameObject price_Ui;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < shop_Slot_Ob.transform.childCount; i++)
        {
            slot_List.Add(shop_Slot_Ob.transform.GetChild(i).GetComponent<Shop_Slot>());
        }
        for (int i = 0; i < inven_Slot_Ob.transform.childCount; i++)
        {
            inven_Slot_List.Add(inven_Slot_Ob.transform.GetChild(i).GetComponent<Inven_Slot>());
        }
    }

    public override void interact()
    {
        base.interact();
        On_Off();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // ���� �¿���
        {
            On_Off();
        }
        else if (Input.GetKeyDown(KeyCode.K)) // ���� �Ǹ� ������ ����
        {
            Update_Slot();
        }
    }

    public void All_Off()
    {
        price_Ui.SetActive(false);
    }

    public void On_Off() // ���� ���� �ѱ�
    {
        // �κ��丮 ����
        if (shop_Panel.GetComponent<RectTransform>().localScale.x == 1)
        {
            // ���� �κ��丮 -> �κ��丮�� ����
            Shop_Invent();

            money_Slot = null;
            All_Off();
            shop_Panel.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
        else // �ѱ�
        {
            shop_Panel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            // �κ��丮 -> ���� �κ��丮�� ����
            Invent_Shop();
        }
    }

    public void Shop_Invent()
    {
        // ���� �κ��丮 -> �κ��丮�� ����
        for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
        {
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
        }

        csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Money_Slot_Find();
        csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Update();
    }

    public void Invent_Shop()
    {
        csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Money_Slot_Find();

        // �κ��丮 -> ���� �κ��丮�� ����
        for (int i = 0; i < inven_Slot_List.Count; i++)
        {
            inven_Slot_List[i].Update_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].item, csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].have_Count);

            if (inven_Slot_List[i].item != null)
            {
                // id�� 0�� ��
                if (inven_Slot_List[i].item.id == 0)
                {
                    money_Slot = inven_Slot_List[i];

                    money = inven_Slot_List[i].have_Count;

                    money_T.text = inven_Slot_List[i].have_Count.ToString("N0");
                }
            }
        }
        
        csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Miri_Inven_Update();
    }

    public Item_Info find_Item(string name)
    {
        for(int i = 0; i < all_Item_List.Count; i++)
        {
            if (all_Item_List[i].item_Name == name)
            {
                return all_Item_List[i];
            }
        }
        return null;
    }

    public void UnRock_Slot() // �������� ������� ���� ����
    {
        for(int i = 0; i < slot_List.Count; i++)
        {
            if (slot_List[i].gameObject.activeSelf == false)
            {
                slot_List[i].gameObject.SetActive(true);
            }
        }
    }

    public void Update_Slot() // �Ǹ��� ������ ǥ��
    {
        // �⺻ �̾���, �ϼ� ������
        int base_N = 0, combi_N = 0;

        int select_N = 0;

        for (int i = 0; i < slot_List.Count; i++)
        {
            if (slot_List[i].gameObject.activeSelf == true)
            {
                if(i < 6)
                {
                    if (combi_N < 2) // �� �ΰ��� ����
                    {
                        select_N = CheckDuplicate(combination_Item_List, select_N);
                        slot_List[i].Update_Slot(combination_Item_List[select_N]);
                        Update_Slot_ClientRpc(combination_Item_List[select_N].id, i);
                        combi_N++;
                    }
                    else if (base_N < 2)
                    {
                        select_N = CheckDuplicate(base_Item_List, select_N);
                        slot_List[i].Update_Slot(base_Item_List[select_N]);
                        Update_Slot_ClientRpc(base_Item_List[select_N].id, i);
                        base_N++;
                    }
                    else
                    {
                        //int rand = Random.Range(0, all_Item_List.Count);
                        select_N = CheckDuplicate(all_Item_List, select_N);
                        slot_List[i].Update_Slot(all_Item_List[select_N]);
                        Update_Slot_ClientRpc(all_Item_List[select_N].id, i);
                    }
                }
                else // ������� �������� ������� ������ �������� ���� ����ȭ X
                {
                    int rand = Random.Range(0, all_Item_List.Count);
                    //select_N = CheckDuplicate(all_Item_List, select_N);
                    slot_List[i].Update_Slot(all_Item_List[rand]);
                    //Update_Slot_ClientRpc(all_Item_List[select_N].id, i);
                }
            }
        }
    }

    [ClientRpc]
    void Update_Slot_ClientRpc(int id, int index) // ������ ����, Ŭ���̾�Ʈ ����ȭ
    {
        if (IsServer)
            return;

        print("Ŭ��");


        for (int j = 0; j < all_Item_List.Count; j++)
        {
            if (all_Item_List[j].id == id)
            {
                GameObject.Find("Shop_Manager").GetComponent<Shop_Manager>().slot_List[index].Update_Slot(all_Item_List[j]);
                break;
            }
        }
    }

    public int CheckDuplicate(List<Item_Info> list, int select_N) // �ߺ� üũ
    {
        int checkDuplicate = Random.Range(0, list.Count);

        // slot_List�� ���� �������� �ִ��� Ȯ��
        bool isDuplicate = false;

        foreach (var slot in slot_List)
        {
            if (slot.item != null)
            {
                if (slot.item.id == list[checkDuplicate].id)
                {
                    isDuplicate = true; // �ߺ� �߰�
                    break;
                }
            }
            else
            {
                isDuplicate = false;
                break;
            }
        }

        // �ߺ��� ������ �ݺ� ����
        if (isDuplicate == false)
        {
            return checkDuplicate; // ���õ� ������ ��ȯ
        }
        else
        {
           return CheckDuplicate(list, select_N);
        }
    }

    public void Money_Up(Item_Info item, int price)
    {
        for (int i = 0; i < inven_Slot_List.Count; i++)
        {
            if (inven_Slot_List[i].item == null)
            {
                inven_Slot_List[i].Update_Slot(item, price);
                money_Slot = inven_Slot_List[i];
                break;
            }
        }
    }
}
