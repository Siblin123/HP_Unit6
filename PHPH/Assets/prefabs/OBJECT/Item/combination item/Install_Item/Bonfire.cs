using Unity.Netcode;
using UnityEngine;

public class Bonfire : Item_Info
{
    public override void UseItem()
    {
        if (!csTable.Instance.gameManager.player.IsOwner)
            return;

        base.UseItem();
        //base.Obj_Installable(id);
        Obj_Installablee();
  
    }

    public void Obj_Installablee()//������Ʈ ��ġ ============��ġ �������� ��� ��� ���������������������
    {
      
        Obj_Installablee_ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void Obj_Installablee_ServerRpc()
    {

        print("123");
        GameObject g = Instantiate(gameObject);
        print("$$$");

    }
}
