using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerGadget : NetworkBehaviour
{

    public Item_Info curItem;
    public NetworkAnimator Arm_Anim;


    public virtual void Start()
    {
    
        
    }

    public virtual void FixedUpdate()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (IsOwner)
            if (Input.GetMouseButtonDown(0))
            {

                UseCurItem();
            }
    }

    public virtual void init()
    {

    }

    public void UseCurItem()
    {
        if(curItem != null)
            curItem.UseItem();
    }

}
