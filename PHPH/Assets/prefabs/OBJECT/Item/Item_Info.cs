using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : MonoBehaviour
{
    public string name; // 아이템 이름
    public int id; // 아이템 아이디
    public string explan; // 설명
    public int price; // 가격
    public int max_Have_Count; // 최대 소지 개수
    

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

    public virtual void UseItem()//각 아이템의 기능
    {

    }
}
