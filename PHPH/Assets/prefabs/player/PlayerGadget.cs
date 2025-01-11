using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerGadget : NetworkBehaviour
{

    public Item_Info curItem;
    public Arm_Anim arm_Anim;

    public float behaviourColTimme;//Çàµ¿ ÄðÅ¸ÀÓ
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
        Change_MiniInventory();

        if (behaviourColTimme>=0)
            behaviourColTimme-= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && behaviourColTimme <= 0 )
        {
            UseCurItem();
        }
    }

    public virtual void init()
    {

    }

    public void Change_MiniInventory()
    {
        if (!IsOwner)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            print("1");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            print("2");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            print("3");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            print("4");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            print("5");
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            print("6");





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
