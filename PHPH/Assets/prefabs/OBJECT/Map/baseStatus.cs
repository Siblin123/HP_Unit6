using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class baseStatus : interaction
{
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(100);
    public NetworkVariable<int> health;

    public float damege;

    [Header("지식의 기억 드랍 아이템 리스트 및 확률")]
    public int reward_memory_Item_Per;
    public List<Item_Info> reward_memory_Item; //지식의 기억으로 얻는 아이템

    public enum tag_Type
    {
        Enemy,
        Object,
        
    }

    public tag_Type tagType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame

    public void TakeDamage(int damege)
    {
        print("TakeDamage");
        if (IsServer)
            health.Value -= damege;
        else
            TakeDamage_ServerRpc(damege);


        if (health.Value <= 0)
        {
            objDestory_ServerRpc();

        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage_ServerRpc(int damege)
    {
        health.Value -= damege;
    }



    [ServerRpc(RequireOwnership = false)]
    public void objDestory_ServerRpc()
    {
        if (!IsServer)
            return;


        //아이템 생성
        SpawnItem();

        GetComponent<NetworkObject>().Despawn(true);

    }


    public void SpawnItem()
    {
        if (reward_memory_Item.Count!=0)
        {

            // ===================  특수 아이템 드랍 ==========================
            int randomNum = Random.Range(1, reward_memory_Item.Count);

            if(randomNum>reward_memory_Item_Per)
            {
                GameObject itemm = Instantiate(reward_memory_Item[randomNum].gameObject, transform.position, Quaternion.identity);

                var networkObjectt = itemm.GetComponent<NetworkObject>();
                if (networkObjectt != null)
                {
                    networkObjectt.Spawn(); // 네트워크 객체로 생성

                    // 서버에서 AddForce 호출 후 클라이언트 동기화
                    Vector2 force = Vector2.up * 5; // 서버에서 정의된 force
                    ApplyForce_ClientRpc(networkObjectt.NetworkObjectId, force);

                }
            }






            // 아이템 생성 및 초기화 ============ 기본 드랍 아이템 ======================
            GameObject item = Instantiate(reward_memory_Item[0].gameObject, transform.position, Quaternion.identity);
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
    public void ApplyForce_ClientRpc(ulong networkObjectId, Vector2 force)
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
