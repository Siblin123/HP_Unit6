using Unity.Netcode;
using UnityEngine;

public class MapObject : baseStatus
{
    public Item_Info reward_Item; // 획득할 아이템 정보

    public override void interact()
    {
        Destroy(gameObject);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsServer)
        {
            SpawnItem();
        }
        else
        {
            getItem_ServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void getItem_ServerRpc()
    {
        // 서버에서 처리 후 클라이언트에 브로드캐스트
        SpawnItem();
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

                // 서버에서 AddForce 호출 후 클라이언트 동기화
                Vector2 force = Vector2.up * 5; // 서버에서 정의된 force
                ApplyForce_ClientRpc(networkObject.NetworkObjectId, force);
               
            }
        }
        else
        {
            Debug.LogWarning("Reward Item is null!");
        }
    }

    [ClientRpc]
    private void ApplyForce_ClientRpc(ulong networkObjectId, Vector2 force)
    {
        // 네트워크 객체를 클라이언트에서 찾아서 AddForce 적용
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null && networkObject.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            print("@@@@@@@@@@@@@@ASD");
        }
    }
}
