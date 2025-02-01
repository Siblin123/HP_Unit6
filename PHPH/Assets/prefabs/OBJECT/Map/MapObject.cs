using Unity.Netcode;
using UnityEngine;

public class MapObject : baseStatus
{
    public Item_Info reward_Item; // ȹ���� ������ ����

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
        // �������� ó�� �� Ŭ���̾�Ʈ�� ��ε�ĳ��Ʈ
        SpawnItem();
    }

    
}
