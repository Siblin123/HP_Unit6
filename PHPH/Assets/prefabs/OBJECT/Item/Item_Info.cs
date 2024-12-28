using System.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : NetworkBehaviour
{
    Rigidbody2D rb;
    public string name; // 아이템 이름
    public int id; // 아이템 아이디
    public string explan; // 설명
    public int price; // 가격
    public int max_Have_Count; // 최대 소지 개수
    public float colTime;

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


    private void Update()
    {
        if(colTime>=0)
        {
            colTime -= Time.deltaTime;
        }
       
    }

    public virtual void UseItem()//각 아이템의 기능
    {
        if(colTime<=0)
        {
            return;
        }
    }

    

    [ServerRpc(RequireOwnership = false)]
    public void GetItem_ServerRpc()
    {
        //충돌 시 인벤토리에 아이템을 넣는다 해당 오브젝트는 비활성화
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
