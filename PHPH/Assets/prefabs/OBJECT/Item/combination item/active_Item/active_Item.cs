using UnityEngine;

public class active_Item : combination_item
{
    public int damege;
    public float attack_rage;
    public float CrowdControl_Time;

    RaycastHit2D [] hit;

    public override void UseItem()
    {
        base.UseItem();
        Attack();
    }

    public void Attack()
    {

    }
}
