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
        // 이미 생성된 네트워크 오브젝트 등록
        foreach (var obj in NetworkManager.Singleton.SpawnManager.SpawnedObjects.Values)
        {
            RegisterSpawn(obj);
        }
    }

    public static void RegisterSpawn(NetworkObject obj)
    {
        // 이미 리스트에 존재하는지 확인
        if (!spawnedObjectsss.Contains(obj.NetworkObjectId))
        {
            Debug.Log($"{obj.name}이(가) Spawn됨!");
            OnSpawned?.Invoke(obj);
            spawnedObjectsss.Add(obj.NetworkObjectId);
        }
        else
        {
            Debug.Log($"{obj.name}은 이미 Spawn된 상태입니다.");
        }
    }

    public static void RegisterDespawn(NetworkObject obj)
    {
        Debug.Log($"{obj.name}이(가) Despawn됨!");

        // 리스트에서 오브젝트 제거
        if (spawnedObjectsss.Contains(obj.NetworkObjectId))
        {
            spawnedObjectsss.Remove(obj.NetworkObjectId);
        }

        OnDespawned?.Invoke(obj);
    }


    //↓↓↓↓↓↓↓↓↓↓↓↓=============================특정 오브젝트 찾기   =============================↓↓↓↓↓↓↓↓↓↓↓
    public NetworkObject Find_NetworkObject(ulong networkObjectId)//매개변수 네트워크 아이디
    {
        if (spawnedObjectsss.Contains(networkObjectId))
        {
            // 네트워크 아이디에 해당하는 네트워크 오브젝트를 반환
            foreach (var obj in NetworkManager.Singleton.SpawnManager.SpawnedObjects.Values)
            {

                    print(obj.name);
                    if (obj.NetworkObjectId == networkObjectId)
                    {
                        return obj;  // 해당 네트워크 오브젝트 반환
                    }
                
              
            }
        }

        // 해당 네트워크 아이디로 찾을 수 없는 경우 null 반환
        return null;
    }


}

//====================================↑네트워크 오브젝트 추가===========================================================

/* 
 * 
 * 네트워크 오브젝트 추가 하려며  NetworkObject를 상속받은 스크립트에 아래와 같이 추가해주세요.
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


//====================================↑네트워크 오브젝트 추가===========================================================