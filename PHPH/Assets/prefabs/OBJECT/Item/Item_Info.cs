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

    public virtual void Update()
    {

    }


    public virtual void UseItem()//각 아이템의 기능
    {
        if(csTable.Instance.gameManager.player.behaviourColTimme <= 0)
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
    public void GetItem_ServerRpc(NetworkObjectReference playerRef)
    {
        // 충돌 시 인벤토리에 아이템을 넣는다. 해당 오브젝트는 비활성화
        print("GetItem_ServerRpc");

        if (playerRef.TryGet(out NetworkObject playerNetworkObject))
        {
            var playerInventory = playerNetworkObject.GetComponent<Player_Inventory>();
            if (playerInventory != null)
            {
                playerInventory.Get_Item(this, 1);
            }
        }

        // 모든 클라이언트에서 이 아이템을 비활성화
        GetItem_ClientRpc();
    }

    [ClientRpc]
    public void GetItem_ClientRpc()
    {
        print("GetItem_ClientRpc");
        // 아이템 비활성화 처리
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.GetComponent<NetworkObject>())
            return;

        if (collision.transform.CompareTag("Player"))
        {
            var playerNetworkObject = collision.transform.GetComponent<NetworkObject>();

            if (IsServer)
            {
                print("IsServer");
                // 서버에서 아이템 획득 처리
                GetItem_ServerRpc(playerNetworkObject);
            }
            else
            {
                print("IsClient");
                // 클라이언트에서 서버로 아이템 획득 요청
                GetItem_ServerRpc(new NetworkObjectReference(playerNetworkObject));
            }
        }
    }

}
