using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class baseStatus : interaction
{
    public NetworkVariable<float> maxHealth = new NetworkVariable<float>(100);
    public NetworkVariable<float> health;

    public float damege;

    [Header("������ ��� ��� ������ ����Ʈ �� Ȯ��")]
    public int reward_memory_Item_Per;
    public List<Item_Info> reward_memory_Item; //������ ������� ��� ������

    //�������� ������
    Item_Info spawnItem;
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

    public void TakeDamage(float damege)
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
    public void TakeDamage_ServerRpc(float damege)
    {
        health.Value -= damege;
    }



    [ServerRpc(RequireOwnership = false)]
    public void objDestory_ServerRpc()
    {
        if (!IsServer)
            return;


        //������ ����
        SpawnItem();

        GetComponent<NetworkObject>().Despawn(true);

    }
    float reward_memory_SpawnPer()//Ư�� �����۵�� Ȯ��
    {
        int randomNum = Random.Range(1, reward_memory_Item.Count);//Ư�� �������� �������� �̱�
        spawnItem = reward_memory_Item[randomNum];
        switch (spawnItem.id)
        {
            //������ ���� Ư�� ������

            case 9://���̾Ƹ��
                return csTable.Instance.gameManager.player.get_mining_Item_Per;


            //���÷� ���� Ư�� ������
            case 14://��� 
                return csTable.Instance.gameManager.player.get_fishing_Item_Per;


            //ä������ ���� Ư�� ������
            case 15://���
                return csTable.Instance.gameManager.player.get_gethering_Item_Per;


            //������� ���� Ư�� ������
            /*case 16://�䳢
                return csTable.Instance.gameManager.player.get_hunting_Item_Per;*/

            default:
                return 0;
        }

    }

    public void SpawnItem()
    {
        if (reward_memory_Item.Count!=0)
        {

            // ===================  Ư�� ������ ��� ==========================

            if (reward_memory_Item.Count > 1)
            {
                int randomNum = Random.Range(1, 101);//�������� Ư�� �������� �� Ȯ��
                if (randomNum <= reward_memory_SpawnPer())//Ư�������� �����ָ鼭 Ȯ�� ����
                {
                    GameObject itemm = Instantiate(spawnItem.gameObject, transform.position, Quaternion.identity);

                    var networkObjectt = itemm.GetComponent<NetworkObject>();
                    if (networkObjectt != null)
                    {
                        networkObjectt.Spawn(); // ��Ʈ��ũ ��ü�� ����

                        // �������� AddForce ȣ�� �� Ŭ���̾�Ʈ ����ȭ
                        Vector2 force = Vector2.up * 5; // �������� ���ǵ� force
                        ApplyForce_ClientRpc(networkObjectt.NetworkObjectId, force);

                    }
                }
            }







            // ������ ���� �� �ʱ�ȭ ============ �⺻ ��� ������ ======================
            GameObject item = Instantiate(reward_memory_Item[0].gameObject, transform.position, Quaternion.identity);
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
    public void ApplyForce_ClientRpc(ulong networkObjectId, Vector2 force)
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
