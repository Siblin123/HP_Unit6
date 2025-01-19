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
        if (!IsOwner)
            return;

        if (!GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        }
        f_ItemPos = transform.position;
      
    }

    public virtual void Update()
    {

    }


    public void Obj_Installable(GameObject netobj)//������Ʈ ��ġ ============��ġ �������� ��� ��� ���������������������
    {


        if (IsServer)
        {
            GameObject obj = Instantiate(netobj, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
            obj.GetComponent<NetworkObject>().Spawn();
        }
        else
        {
            Obj_Installable_ServerRpc(netobj.GetComponent<Item_Info>().id);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void Obj_Installable_ServerRpc(int id)
    {

        if (IsClient)
            return;

        foreach(var spawn_Obj in csTable.Instance.allItem_List)
        {
            if(spawn_Obj.id == id)
            {
                GameObject obj = Instantiate(spawn_Obj.gameObject, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
                obj.GetComponent<NetworkObject>().Spawn();
                break;
            }
        }

    

    }                  
                //������Ʈ ��ġ ============��ġ �������� ��� ��� ������������������������

    public virtual void UseItem()//�� �������� ���
    {
        if (!IsOwner)
        {
            return;
        }

        if (csTable.Instance.gameManager.player.behaviourColTimme <= 0)
        {
            csTable.Instance.gameManager.player.behaviourColTimme= colTime;

            return;
        }
    }

    public virtual void Attack()
    {
        if (csTable.Instance.gameManager.player.behaviourColTimme <= 0)
        {
            csTable.Instance.gameManager.player.behaviourColTimme = colTime;
            return;
        }
    }

   [ServerRpc(RequireOwnership = false)]
    public void GetItem_ServerRpc()
    {

        GetItem_ClientRpc();

    }

    [ClientRpc]
    public void GetItem_ClientRpc( )
    {
       
        gameObject.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       /* if (!collision.transform.GetComponent<NetworkObject>())
            return;*/

        if (collision.transform.CompareTag("Player"))
        {
            //var playerNetworkObject = collision.transform.GetComponent<NetworkObject>();
            collision.transform.GetComponent<Player_Inventory>().Get_Item(this, 1);
           
            if (IsServer)
            {
                GetItem_ClientRpc();
            }
            else
            {
                GetItem_ServerRpc();
            }
        }
    }

}
