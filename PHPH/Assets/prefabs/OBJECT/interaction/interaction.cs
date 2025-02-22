using Unity.Netcode;
using UnityEngine;

public class interaction : NetworkBehaviour
{
    public void Awake()
    {
       
        
    }
    //====================================↑네트워크 오브젝트 추가===========================================================

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkSpawnManager.RegisterSpawn(NetworkObject);
        print("find _ new _ obj");
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        NetworkSpawnManager.RegisterDespawn(NetworkObject);
    }


    //====================================↑네트워크 오브젝트 추가===========================================================
    public virtual void interact()
    {
        
    }

    public void Open_UI(GameObject UI)
    {
        //UI 열어주기
    }

}
