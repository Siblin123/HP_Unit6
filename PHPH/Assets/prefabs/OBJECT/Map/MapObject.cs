using Unity.Netcode;
using UnityEngine;

public class MapObject : baseStatus
{
    //public Item_Info reward_Item; // ȹ���� ������ ����

    public override void interact()
    {
        Destroy(gameObject);
    }
    
}
