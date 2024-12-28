using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inven_Slot : MonoBehaviour
{
    public Item_Info item; // ������ ������
    public int have_Count; // ������ ������ ����

    public Image item_I;
    public TextMeshProUGUI count_T;
    public GameObject follow_Slot; // ���콺 ����ٴϴ� ����

    private RectTransform rectTransform;
    public bool follow_C;

    private void Start()
    {
        follow_Slot = GameObject.Find("Follow_Slot"); // ī�޶� ����ٴ� ������Ʈ
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (follow_C)
        {
            Vector2 mousePosition = Input.mousePosition;

            // RectTransform�� ��ġ�� ���콺 ��ġ�� �����մϴ�.
            rectTransform.position = mousePosition;
        }
    }

    public void Update_Slot(Item_Info item, int count) // ���� �ʱ�ȭ
    {
        this.item = item;
        have_Count = count;
       
        if (count_T != null) // ���� ���� ǥ�� ����
        {
            count_T.text = count.ToString();
        }

        item_I.enabled = true;
        item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void Click_Slot() // ��ư Ŭ��������
    {
        follow_Slot.GetComponent<Inven_Slot>().Update_Slot(item, have_Count);
    }
}
