using Unity.Netcode;
using UnityEngine;

public class baseStatus : interaction
{
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(100);
    public NetworkVariable<int> health;

    public enum tag_Type
    {
        Enemy,
        Object
    }

    public tag_Type tagType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damege)
    {
        print("TakeDamage");
        if(IsServer)
            health.Value -= damege;
        else
            TakeDamage_ServerRpc(damege);


        if (health.Value <= 0)
        {
           if(IsServer)
            {
                Die();
                Die_ClientRpc();
            }
            else
            {
                Die();
                Die_ServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage_ServerRpc(int damege)
    {
        health.Value -= damege;
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void Die_ServerRpc()
    {
        Die();
    }

    [ClientRpc]
    public void Die_ClientRpc()
    {
        Die();
    }
}
