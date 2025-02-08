using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSpawnManager : MonoBehaviour
{
    public static event Action<NetworkObject> OnSpawned;
    public static event Action<NetworkObject> OnDespawned;

    [SerializeField]
    public static List<ulong> spawnedObjectsss = new List<ulong>();
    public List<ulong> spawnobj = new List<ulong>();
    private void Update()
    {
        spawnobj= spawnedObjectsss;
    }

    private void Awake()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        };
    }

    private void OnClientConnected(ulong clientId)
    {
        // �̹� ������ ��Ʈ��ũ ������Ʈ ���
        foreach (var obj in NetworkManager.Singleton.SpawnManager.SpawnedObjects.Values)
        {
            RegisterSpawn(obj);
        }
    }

    public static void RegisterSpawn(NetworkObject obj)
    {
        // �̹� ����Ʈ�� �����ϴ��� Ȯ��
        if (!spawnedObjectsss.Contains(obj.NetworkObjectId))
        {
            Debug.Log($"{obj.name}��(��) Spawn��!");
            OnSpawned?.Invoke(obj);
            spawnedObjectsss.Add(obj.NetworkObjectId);
        }
        else
        {
            Debug.Log($"{obj.name}�� �̹� Spawn�� �����Դϴ�.");
        }
    }

    public static void RegisterDespawn(NetworkObject obj)
    {
        Debug.Log($"{obj.name}��(��) Despawn��!");

        // ����Ʈ���� ������Ʈ ����
        if (spawnedObjectsss.Contains(obj.NetworkObjectId))
        {
            spawnedObjectsss.Remove(obj.NetworkObjectId);
        }

        OnDespawned?.Invoke(obj);
    }


    //�������������=============================Ư�� ������Ʈ ã��   =============================������������
    public NetworkObject Find_NetworkObject(ulong networkObjectId)//�Ű����� ��Ʈ��ũ ���̵�
    {
        if (spawnedObjectsss.Contains(networkObjectId))
        {
            // ��Ʈ��ũ ���̵� �ش��ϴ� ��Ʈ��ũ ������Ʈ�� ��ȯ
            foreach (var obj in NetworkManager.Singleton.SpawnManager.SpawnedObjects.Values)
            {

                    print(obj.name);
                    if (obj.NetworkObjectId == networkObjectId)
                    {
                        return obj;  // �ش� ��Ʈ��ũ ������Ʈ ��ȯ
                    }
                
              
            }
        }

        // �ش� ��Ʈ��ũ ���̵�� ã�� �� ���� ��� null ��ȯ
        return null;
    }


}

//====================================���Ʈ��ũ ������Ʈ �߰�===========================================================

/* 
 * 
 * ��Ʈ��ũ ������Ʈ �߰� �Ϸ���  NetworkObject�� ��ӹ��� ��ũ��Ʈ�� �Ʒ��� ���� �߰����ּ���.
 * 
 * 
 * 
 * public override void OnNetworkSpawn()
  {
      base.OnNetworkSpawn();
      NetworkSpawnManager.RegisterSpawn(NetworkObject);
      print("find _ new _ obj");
  }

  public override void OnNetworkDespawn()
  {
      base.OnNetworkDespawn();
      NetworkSpawnManager.RegisterDespawn(NetworkObject);
  }*/


//====================================���Ʈ��ũ ������Ʈ �߰�===========================================================