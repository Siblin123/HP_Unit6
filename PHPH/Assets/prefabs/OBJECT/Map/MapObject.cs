using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;

public class MapObject : interaction
{
    public Item_Info reward_Item; // 획득할 아이템 정보

    public override void interact()
    {
        if (IsServer)
        {
            // 서버에서 직접 처리
            SpawnItem();

            Destroy(gameObject);
        }
        else
        {
            // 클라이언트에서 서버 RPC 호출
            getItem_ServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void getItem_ServerRpc()
    {
        // 서버에서 처리 후 클라이언트에 브로드캐스트
        SpawnItem();
        getItem_ClientRpc();
       
    }

    [ClientRpc]
    public void getItem_ClientRpc()
    {
        // 클라이언트에서 추가적인 처리 (UI, 효과 등)
   

        Destroy(gameObject);
    }

    private void SpawnItem()
    {
        if (reward_Item != null)
        {
            // 아이템 생성 및 초기화
            GameObject item = Instantiate(reward_Item.gameObject, transform.position, Quaternion.identity);
            var networkObject = item.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn(); // 네트워크 객체로 생성
            }


        }
        else
        {
            Debug.LogWarning("Reward Item is null!");
           
        }
    }
}
