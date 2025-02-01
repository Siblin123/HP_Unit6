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

    
}
