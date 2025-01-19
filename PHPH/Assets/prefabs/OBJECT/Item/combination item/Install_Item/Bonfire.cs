using Unity.Netcode;
using UnityEngine;

public class Bonfire : Item_Info
{
    public override void UseItem()
    {
        if (!csTable.Instance.gameManager.player.IsOwner)
            return;

        base.UseItem();
        
  
    }


}
