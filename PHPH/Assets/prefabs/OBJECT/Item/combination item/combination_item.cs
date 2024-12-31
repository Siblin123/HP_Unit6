using Unity.Netcode.Components;
using UnityEngine;

public class combination_item : Item_Info
{
    public NetworkAnimator anim;
    public AnimationClip clip;
    public Arm_Anim.ArmType ArmType;

    public override void UseItem()
    {
        base.UseItem();
        Arm_Anim();
    }


    public virtual void Arm_Anim()
    {
        csTable.Instance.gameManager.player.arm_Anim.Anim = ArmType;
        print("µµ³¢µµ³¢");
    }


}
