using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : MonoBehaviour
{

    public enum itemType
    {
        base_Item,
        Grand_Item,
        combination_Item_Gadget,//���
        combination_Item_active,//��Ƽ��(�����)
        combination_Item_Installable,//��ġ��
        combination_Item_consumable,//�Һ���


    }
    public itemType curItemType;

    public virtual void UseItem()//�� �������� ���
    {

    }
}
