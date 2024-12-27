using System.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : NetworkBehaviour
{
    Rigidbody2D rb;
    public string name; // ������ �̸�
    public int id; // ������ ���̵�
    public string explan; // ����
    public int price; // ����
    public int max_Have_Count; // �ִ� ���� ����
    public float colTime;

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
        if (!GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        }
        f_ItemPos = transform.position;
      
    }


    private void Update()
    {
        if(colTime>=0)
        {
            colTime -= Time.deltaTime;
        }
       
    }

    public virtual void UseItem()//�� �������� ���
    {
        if(colTime<=0)
        {
            return;
        }
    }

    

    [ServerRpc(RequireOwnership = false)]
    public void GetItem_ServerRpc()
    {
        //�浹 �� �κ��丮�� �������� �ִ´� �ش� ������Ʈ�� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    [ClientRpc]
    public void GetItem_ClientRpc()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.GetComponent<NetworkObject>())
            return;

        if (collision.transform.CompareTag("Player"))
        {
            if(IsServer)
            {
                GetItem_ClientRpc();
                gameObject.SetActive(false);
            }
            else
            {
                GetItem_ServerRpc();
                gameObject.SetActive(false);
            }
        }
    }

}
