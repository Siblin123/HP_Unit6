using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Item_Info;

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
        {

            if(curItem.curItemType== itemType.combination_Item_Installable)
            {
                Obj_Installable(curItem.id);
            }
            else
            {
                curItem.UseItem();
            }

         
        }
           
    }

    public void UseCurItem_Attack()
    {
        if (!IsOwner)
            return;
        if (curItem != null)
            curItem.Attack();
    }




    public void Obj_Installable(int id)//오브젝트 설치 ============설치 아이템일 경우 사용 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    {

        Obj_Installable_ServerRpc(id, csTable.Instance.gameManager.player.transform.position);

        /*            GameObject obj = Instantiate(netobj, csTable.Instance.gameManager.player.transform.position, Quaternion.identity);
                    obj.GetComponent<NetworkObject>().Spawn();*/


    }

    [ServerRpc(RequireOwnership = false)]
    public void Obj_Installable_ServerRpc(int id, Vector3 pos)
    {
        print("find obj");

        foreach (var spawn_Obj in csTable.Instance.allItem_List)
        {
            if (spawn_Obj.id == id)
            {
                GameObject obj = Instantiate(spawn_Obj.gameObject, pos, Quaternion.identity);
                obj.GetComponent<NetworkObject>().Spawn();
                break;
            }
        }



    }
    //오브젝트 설치 ============설치 아이템일 경우 사용 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


}
