using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class Item_Info : MonoBehaviour
{
    public string name; // ������ �̸�
    public int id; // ������ ���̵�
    public string explan; // ����
    public int price; // ����
    public int max_Have_Count; // �ִ� ���� ����
    

    public enum itemType
    {
        base_Item,
        Grand_Item,
        combination_Item_Gadget,//���
        combination_Item_active,//��Ƽ��(�����)
        combination_Item_Installable,//��ġ��
        combination_Item_consumable,//�Һ���
        Memory_Knowledge, // ������ ���

    }
    public itemType curItemType;

    public virtual void UseItem()//�� �������� ���
    {

    }
}
