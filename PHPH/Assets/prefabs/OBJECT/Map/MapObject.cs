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

    private void SpawnItem()
    {
        if (reward_Item != null)
        {
            // ������ ���� �� �ʱ�ȭ
            GameObject item = Instantiate(reward_Item.gameObject, transform.position, Quaternion.identity);
            var networkObject = item.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn(); // ��Ʈ��ũ ��ü�� ����

                // �������� AddForce ȣ�� �� Ŭ���̾�Ʈ ����ȭ
                Vector2 force = Vector2.up * 5; // �������� ���ǵ� force
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
        // ��Ʈ��ũ ��ü�� Ŭ���̾�Ʈ���� ã�Ƽ� AddForce ����
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        if (networkObject != null && networkObject.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            print("@@@@@@@@@@@@@@ASD");
        }
    }
}
