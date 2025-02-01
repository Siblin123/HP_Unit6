using Unity.Netcode;
using UnityEngine;

public class MapObject : baseStatus
{
    //public Item_Info reward_Item; // »πµÊ«“ æ∆¿Ã≈€ ¡§∫∏

    public override void interact()
    {
        Destroy(gameObject);
    }
    
}
