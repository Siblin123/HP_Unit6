using UnityEngine;

public class Bonfire : Item_Info
{
    public override void UseItem()
    {
        base.UseItem();
        base.Obj_Installable();
    }


}
