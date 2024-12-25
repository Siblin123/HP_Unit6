using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : MonoBehaviour
{
    Rigidbody2D rb;
    public string name; // 아이템 이름
    public int id; // 아이템 아이디
    public string explan; // 설명
    public int price; // 가격
    public int max_Have_Count; // 최대 소지 개수


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
        rb = GetComponent<Rigidbody2D>();
        f_ItemPos= transform.position;
    }

    private void Update()
    {
        if (transform.position.y < f_ItemPos.y && rb.bodyType != RigidbodyType2D.Static)
        {
            transform.position = new Vector3(transform.position.x, f_ItemPos.y, transform.position.z);
            rb.bodyType = RigidbodyType2D.Static; // 리지드바디의 bodyType을 static으로 변경
        }
    }



    public virtual void UseItem()//각 아이템의 기능
    {

    }

    public virtual void GetItem()
    {
        //충돌 시 인벤토리에 아이템을 넣는다 해당 오브젝트는 비활성화

    }



}
