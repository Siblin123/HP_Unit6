using System.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : NetworkBehaviour
{
    Rigidbody2D rb;
    public string item_Name; // ������ �̸�
    public int id; // ������ ���̵�
    public string explan; // ����
    public int price; // ����
    public int max_Have_Count; // �ִ� ���� ����
    public float colTime;
    public Sprite hand_Item_Sprite; // �տ� ������� �̹���

    public int have_Count; // ���� ���� ����

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

        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        }
        f_ItemPos = transform.position;
      
    }

    //====================================���Ʈ��ũ ������Ʈ �߰�===========================================================

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkSpawnManager.RegisterSpawn(NetworkObject);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        NetworkSpawnManager.RegisterDespawn(NetworkObject);
    }


    //====================================���Ʈ��ũ ������Ʈ �߰�===========================================================

    public virtual void Update()
    {

    }


   
    public virtual void UseItem()//�� �������� ���
    {
 
        if (csTable.Instance.gameManager.player.behaviourColTimme <= 0)
        {
            csTable.Instance.gameManager.player.behaviourColTimme= colTime;
            gatgot_Coltime();
            
        }
    }

    public virtual void Attack()
    {
        if (csTable.Instance.gameManager.player.behaviourColTimme <= 0)
        {
            csTable.Instance.gameManager.player.behaviourColTimme = colTime;

            gatgot_Coltime();

            
        }
    }

   
    public void gatgot_Coltime()
    {
        switch (id)
        {
            case 100:
                //����
                csTable.Instance.gameManager.player.behaviourColTimme -= csTable.Instance.gameManager.player.ax_memory_Value;
                break;
            case 106:
                //���
                csTable.Instance.gameManager.player.behaviourColTimme -= csTable.Instance.gameManager.player.pick_memory_Value;
                break;
        }
    }
}
