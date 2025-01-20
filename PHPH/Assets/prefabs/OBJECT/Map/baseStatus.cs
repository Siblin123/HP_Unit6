using Unity.Netcode;
using UnityEngine;

public class baseStatus : interaction
{
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(100);
    public NetworkVariable<int> health;

    public float damege; 

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



        GetComponent<NetworkObject>().Despawn(true);

    }

}
