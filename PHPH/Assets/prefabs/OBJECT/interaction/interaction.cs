using Unity.Netcode;
using UnityEngine;

public class interaction : NetworkBehaviour
{
    public void Awake()
    {
       
        
    }
    //====================================���Ʈ��ũ ������Ʈ �߰�===========================================================

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


    //====================================���Ʈ��ũ ������Ʈ �߰�===========================================================
    public virtual void interact()
    {
        
    }

    public void Open_UI(GameObject UI)
    {
        //UI �����ֱ�
    }

}
