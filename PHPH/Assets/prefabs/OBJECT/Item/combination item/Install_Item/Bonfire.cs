using Unity.Netcode;
using UnityEngine;

public class Bonfire : Item_Info
{
    [SerializeField]
    private GameObject networkPrefab;
    public override void UseItem()
    {
        if (!IsOwner)
            return;

        base.UseItem();
        base.Obj_Installable(networkPrefab);
    }


}
