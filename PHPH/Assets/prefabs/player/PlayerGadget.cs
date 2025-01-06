using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerGadget : NetworkBehaviour
{

    public Item_Info curItem;
    public Arm_Anim arm_Anim;

    public float behaviourColTimme;//행동 쿨타임
    public virtual void Start()
    {
        csTable.Instance.Player_Inventory = GetComponent<Player_Inventory>();
        init();

    }

    public virtual void FixedUpdate()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!IsOwner)
            return;
        if(behaviourColTimme>=0)
            behaviourColTimme-= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && behaviourColTimme <= 0 )
        {
            UseCurItem();
        }
    }

    public virtual void init()
    {

    }

    public void UseCurItem()
    {
        if (!IsOwner)
            return;

        if (curItem != null )
            curItem.UseItem();
    }

    public void UseCurItem_Attack()
    {
        if (!IsOwner)
            return;
        if (curItem != null)
            curItem.Attack();
    }

}
