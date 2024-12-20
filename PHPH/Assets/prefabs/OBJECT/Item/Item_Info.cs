using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : MonoBehaviour
{

    public enum itemType
    {
        base_Item,
        Grand_Item,
        combination_Item_Gadget,//장비
        combination_Item_active,//액티브(사용형)
        combination_Item_Installable,//설치류
        combination_Item_consumable,//소비템


    }
    public itemType curItemType;

    public virtual void UseItem()//각 아이템의 기능
    {

    }
}
