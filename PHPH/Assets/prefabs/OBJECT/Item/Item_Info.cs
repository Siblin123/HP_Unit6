using System.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : NetworkBehaviour
{
    Rigidbody2D rb;
    public string item_Name; // 아이템 이름
    public int id; // 아이템 아이디
    public string explan; // 설명
    public int price; // 가격
    public int max_Have_Count; // 최대 소지 개수
    public float colTime;
    public Sprite hand_Item_Sprite; // 손에 들었을때 이미지

    public int have_Count; // 현재 소지 개수

    //아이템  위치 고정 플레이어가 아래로 내려갈때 아이템도 같이 떨어지는거 막는 용도
    Vector3 f_ItemPos;


    public enum itemType
    {
        base_Item,
        Grand_Item,
        combination_Item_Gadget,//장비
        combination_Item_active,//액티브(사용형)
        combination_Item_Installable,//설치류
        combination_Item_consumable,//소비템
        Memory_Knowledge, // 지식의 기억

    }
    public itemType curItemType;

    private void Start()
    {

        if (GetComponent<Rigidbody2D>())
        {
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        }
        f_ItemPos = transform.position;
      
    }

    //====================================↑네트워크 오브젝트 추가===========================================================

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkSpawnManager.RegisterSpawn(NetworkObject);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        NetworkSpawnManager.RegisterDespawn(NetworkObject);
    }


    //====================================↑네트워크 오브젝트 추가===========================================================

    public virtual void Update()
    {

    }


   
    public virtual void UseItem()//각 아이템의 기능
    {
 
        if (csTable.Instance.gameManager.player.behaviourColTimme <= 0)
        {
            csTable.Instance.gameManager.player.behaviourColTimme= colTime;
            gatgot_Coltime();
            
        }
    }

    public virtual void Attack()
    {
        if (csTable.Instance.gameManager.player.behaviourColTimme <= 0)
        {
            csTable.Instance.gameManager.player.behaviourColTimme = colTime;

            gatgot_Coltime();

            
        }
    }

   
    public void gatgot_Coltime()
    {
        switch (id)
        {
            case 100:
                //도끼
                csTable.Instance.gameManager.player.behaviourColTimme -= csTable.Instance.gameManager.player.ax_memory_Value;
                break;
            case 106:
                //곡괭이
                csTable.Instance.gameManager.player.behaviourColTimme -= csTable.Instance.gameManager.player.pick_memory_Value;
                break;
        }
    }
}
