using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.Netcode;

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
    public GameObject money_View;
    public int money;

    // ���� ���� �̹���
    public GameObject price_Ui;

    public NetworkVariable<string> item_Name;

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
        else if (Input.GetKeyDown(KeyCode.D)) // ���� �Ǹ� ������ ����
        {
            Update_Slot();
        }

        if (IsServer)
        {
            item_Name.Value = "Jiho guiyuwa";
        }
      
        
    }

    private void OnEnable()
    {
        // �κ��丮 -> ���� �κ��丮 ����ȭ
        for (int i = 0; i < slot_List.Count; i++)
        {
            inven_Slot_List[i].Update_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].item, Player_Inventory.instance.slot_List[i].have_Count);

            // id�� 100�� ��
            if (inven_Slot_List[i].item.id == 100)
            {
                money = inven_Slot_List[i].item.have_Count;

                money_View.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = inven_Slot_List[i].have_Count.ToString("N0");
            }
        }
    }

    private void OnDisable()
    {
        // ���� �κ��丮 -> �κ��丮 ����ȭ
        for (int i = 0; i < slot_List.Count; i++)
        {
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
        }
    }

    public void All_Off()
    {
        price_Ui.SetActive(false);
    }

    public void On_Off() // ���� ���� �ѱ�
    {
        // �κ��丮 ����
        if (shop_Panel.activeSelf == true)
        {
            // �������� �Ǹ� �Ǵ� �����Ѱ� �κ��丮�� ����
            for (int i = 0; i < csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List.Count; i++)
            {
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].Update_Slot(inven_Slot_List[i].item, inven_Slot_List[i].have_Count);
            }

            shop_Panel.SetActive(false);
        }
        else if (shop_Panel.activeSelf == false) // �ѱ�
        {
            shop_Panel.SetActive(true);

            // �κ��丮�� ���� �κ��丮�� ����
            for (int i = 0; i < inven_Slot_List.Count; i++)
            {
                inven_Slot_List[i].Update_Slot(csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].item, csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[i].have_Count);
            }
        }
    }

    public Item_Info find_Item(string name)
    {
        for(int i = 0; i < all_Item_List.Count; i++)
        {
            if (all_Item_List[i].name == name)
            {
                return all_Item_List[i];
            }
        }
        return null;
    }

    public void Update_Slot() // �Ǹ��� ������ ǥ��
    {
        // �⺻ �̾���, �ϼ� ������
        int base_N = 0, combi_N = 0;

        int select_N = 0;

        for (int i = 0; i < slot_List.Count; i++)
        {
            if (combi_N < 2) // �� �ΰ��� ����
            {
                select_N = CheckDuplicate(combination_Item_List, select_N);
                slot_List[i].Update_Slot(combination_Item_List[select_N]);
                combi_N++;
            }
            else if (base_N < 2)
            {
                select_N = CheckDuplicate(base_Item_List, select_N);
                slot_List[i].Update_Slot(base_Item_List[select_N]);
                base_N++;
            }
            else
            {
                select_N = CheckDuplicate(all_Item_List, select_N);
                slot_List[i].Update_Slot(all_Item_List[select_N]);
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
                if (slot.item.name == list[checkDuplicate].name)
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
            return 0;
        }
    }
}
