using Unity.Netcode;
using UnityEngine;

public class PlayerGadget : NetworkBehaviour
{

    public Item_Info curWeapon;

    public virtual void Start()
    {
    
        
    }

    public virtual void FixedUpdate()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            UseCurItem();
        }
    }

    public virtual void init()
    {

    }

    public void UseCurItem()
    {
        if(curWeapon!=null)
            curWeapon.UseItem();
    }

}
