using System.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : NetworkBehaviour
{
    Rigidbody2D rb;
    public string namee; // 아이템 이름
    public int id; // 아이템 아이디
    public string explan; // 설명
    public int price; // 가격
    public int max_Have_Count; // 최대 소지 개수
    public float colTime;

    public int have_Count; // 현재 소지 개수

    //아이템  위치 고정 플레이어가 아래로 내려갈때 아이템도 같이 떨어지는거 막는 용도
    Vector3 f_ItemPos;


    public enum itemType
    {
        base_Item,
        Grand_Item,
        combination_Item_Gadget,//장비
        combination_Item_active,//액티브(사용형)
        combination_Item_Installable,//설치류
        combination_Item_consumable,//소비템
        Memory_Knowledge, // 지식의 기억

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

    public virtual void Update()
    {

    }


    public void Obj_Installable(GameObject netobj)//오브젝트 설치 ============설치 아이템일 경우 사용 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    {

        Obj_Installable_ServerRpc(netobj.GetComponent<Item_Info>().id);

        /*            GameObject obj = Instantiate(netobj, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
                    obj.GetComponent<NetworkObject>().Spawn();*/


    }

    [ServerRpc(RequireOwnership = false)]
    public void Obj_Installable_ServerRpc(int id)
    {
        print("find obj");

        foreach(var spawn_Obj in csTable.Instance.allItem_List)
        {
            if(spawn_Obj.id == id)
            {
                GameObject obj = Instantiate(spawn_Obj.gameObject, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
                obj.GetComponent<NetworkObject>().Spawn();
a                 break;
            }
        }

    

    }                  
                //오브젝트 설치 ============설치 아이템일 경우 사용 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

    public virtual void UseItem()//각 아이템의 기능
    {
 

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
