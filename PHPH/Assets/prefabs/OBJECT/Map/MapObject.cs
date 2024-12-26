using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;

public class MapObject : interaction
{
    public Item_Info reward_Item; // ȹ���� ������ ����
    int jumpingItem_Value = 2;    // ���� ��

    public override void interact()
    {
        if (IsServer)
        {
            // �������� ���� ó��
            SpawnItem();

            Destroy(gameObject);
        }
        else
        {
            // Ŭ���̾�Ʈ���� ���� RPC ȣ��
            getItem_ServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void getItem_ServerRpc()
    {
        // �������� ó�� �� Ŭ���̾�Ʈ�� ��ε�ĳ��Ʈ
        SpawnItem();
        getItem_ClientRpc();
       
    }

    [ClientRpc]
    public void getItem_ClientRpc()
    {
        // Ŭ���̾�Ʈ���� �߰����� ó�� (UI, ȿ�� ��)
   

        Destroy(gameObject);
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
            }
            var rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(0, jumpingItem_Value), ForceMode2D.Impulse);
            }


        }
        else
        {
            Debug.LogWarning("Reward Item is null!");
           
        }
    }
}
