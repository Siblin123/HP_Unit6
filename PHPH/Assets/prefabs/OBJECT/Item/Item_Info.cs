using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : MonoBehaviour
{
    Rigidbody2D rb;
    public string name; // ������ �̸�
    public int id; // ������ ���̵�
    public string explan; // ����
    public int price; // ����
    public int max_Have_Count; // �ִ� ���� ����


    //������  ��ġ ���� �÷��̾ �Ʒ��� �������� �����۵� ���� �������°� ���� �뵵
    Vector3 f_ItemPos;


    public enum itemType
    {
        base_Item,
        Grand_Item,
        combination_Item_Gadget,//���
        combination_Item_active,//��Ƽ��(�����)
        combination_Item_Installable,//��ġ��
        combination_Item_consumable,//�Һ���
        Memory_Knowledge, // ������ ���

    }
    public itemType curItemType;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        f_ItemPos= transform.position;
    }

    private void Update()
    {
        if (transform.position.y < f_ItemPos.y && rb.bodyType != RigidbodyType2D.Static)
        {
            transform.position = new Vector3(transform.position.x, f_ItemPos.y, transform.position.z);
            rb.bodyType = RigidbodyType2D.Static; // ������ٵ��� bodyType�� static���� ����
        }
    }



    public virtual void UseItem()//�� �������� ���
    {

    }

    public virtual void GetItem()
    {
        //�浹 �� �κ��丮�� �������� �ִ´� �ش� ������Ʈ�� ��Ȱ��ȭ

    }



}
