using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using static Arm_Anim;
using static Item_Info;

public class PlayerGadget : NetworkBehaviour
{
    public Item_Info _curItem; // 선택한 아이템
    public Item_Info curItem
    {
        get => _curItem;
        set
        {
            _curItem = value;
            Change_CurItem();
        }
    }

    public Arm_Anim arm_Anim;

    public float behaviourColTimme;//행동 쿨타임

    [Header("가지고 있는 기억")]
    public List<string> have_Memory;

    [Header("기억으로 인한 쿨타임 감소")]
    public float ax_memory_Value;//도끼 쿨타임
    public float pick_memory_Value;//곡괭이 쿨타임
    public float longAttackDamegeUp_Value;//원거리공격력 증가량
    public float shortAttackDamageUp_Value;//근거리 공격력 증가량

    [Header("채집으로 인한 특수 아이템을 획득확률")]
    public float get_gethering_Item_Per;
    [Header("광질로 인한 특수 아이템 획득확률")]
    public float get_mining_Item_Per;
    [Header("낚시로 인한 특수 아이템 획득확률")]
    public float get_fishing_Item_Per;
    [Header("사냥으로 인한 특수 아이템 획득 확률")]
    public float get_hunting_Item_Per;

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

        Change_CurItem();

        if (behaviourColTimme >= 0)
            behaviourColTimme -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && behaviourColTimme <= 0)
        {
            UseCurItem();
        }
    }

    public virtual void init()
    {

    }

    public void Change_CurItem()
    {
        if (arm_Anim._anim == ArmType.empty_P)
        {
            arm_Anim.GetComponent<Animator>().enabled = false;

            if (curItem != null)
            {
                arm_Anim.GetComponent<SpriteRenderer>().sprite = curItem.GetComponent<Item_Info>().hand_Item_Sprite;
                arm_Anim.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                arm_Anim.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            arm_Anim.GetComponent<Animator>().enabled = true;
        }
    }

    public void UseCurItem()
    {
        if (!IsOwner)
            return;

        if (curItem != null)
        {
            if (curItem.curItemType == itemType.combination_Item_Installable)
            {
                Obj_Installable(curItem.id);
            }
            else
            {
                curItem.UseItem();
            }

            if (curItem.curItemType != itemType.combination_Item_active)
                GetComponent<Player_Inventory>().miri_List[GetComponent<Player_Inventory>().currentSlot].Update_Slot(curItem, curItem.have_Count - 1);
        }
    }

    public void UseCurItem_Attack()
    {
        if (!IsOwner)
            return;
        if (curItem != null)
            curItem.Attack();
    }


    public void Change_GadgetItem()//장비 아이템 장착, 해제시 사용
    {
        if (!IsOwner)
            return;

        


    }








    public void Obj_Installable(int id)//오브젝트 설치 ============설치 아이템일 경우 사용 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    {
        Obj_Installable_ServerRpc(id, csTable.Instance.gameManager.player.transform.position);
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

    //=================아이템 버리기================
    [ServerRpc]
    public void Throw_Item_ServerRpc(ulong id, Vector3 pos, int have_Count)
    {
        print("서버 실행");
        Throw_Item_ClientRpc(id, pos, have_Count);
    }

    [ClientRpc]
    public void Throw_Item_ClientRpc(ulong id, Vector3 pos, int have_Count)
    {   
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out NetworkObject networkObject))
        {
            print("클라 서버 모두 실행");
            networkObject.transform.gameObject.SetActive(true);
            networkObject.transform.position = pos;
            networkObject.GetComponent<Item_Info>().have_Count = have_Count;
        }
        else
        {

            foreach (var spawn_Obj in csTable.Instance.allItem_List)
            {
                if (spawn_Obj.id == (int)id)
                {
                    GameObject item = Instantiate(spawn_Obj.gameObject);
                    item.GetComponent<NetworkObject>().Spawn();
                    item.transform.position = pos;
                    item.GetComponent<Item_Info>().have_Count = have_Count;
                    break;
                }
            }


        }
    }
}

