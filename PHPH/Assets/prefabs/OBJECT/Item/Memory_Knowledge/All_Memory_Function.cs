using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class All_Memory_Function : MonoBehaviour
{

    public List<Item_Info> all_Memory; 


    public void all_Function(Item_Info item)
    {
        switch (item.name)
        {
            case "�Ѿ��� ���":
                // �Ѿ� ���� ����
                Debug.Log("�Ѿ��� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "���� �������� ���":
                // ������ ���� ����
                Debug.Log("���� �������� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "�Ź��� ���":
                // �Ź� ���� ����
                Debug.Log("�Ź��� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "�������� ���":
                // ������ ���� ����
                Debug.Log("�������� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "���� ���":
                // �� ���� ����
                Debug.Log("���� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "����Ű�� ���":
                // ����Ű ���� ����
                Debug.Log("����Ű�� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "��ȯ���� ���":
                // ��ȯ�� ���� ����
                Debug.Log("��ȯ���� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "������ ���":
                // ���� ���� ����
                Debug.Log("������ ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "���� ���":
                // �� ���� ����
                Debug.Log("���� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "������ ���":
                // ���� ���� ����
                Debug.Log("������ ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "����� ���":
                // ��� ���� ����
                Debug.Log("����� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "������ ���":
                // ���� ���� ����
                Debug.Log("������ ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "�������� ���":
                // ������ ���� ����
                Debug.Log("�������� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "���� ������ ���":
                // ���� ���� ���� ����
                Debug.Log("���� ������ ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "���ݼ��� ���":
                // ���ݼ� ���� ����
                Debug.Log("���ݼ��� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "�������� ���":
                // ������ ���� ����
                Debug.Log("�������� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "�ɸ����� ���":
                // �ɸ��� ���� ����
                Debug.Log("�ɸ����� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "���°��� ���":
                // ���°� ���� ����
                Debug.Log("���°��� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "��Ƽ���� ���":
                // ��Ƽ�� ���� ����
                Debug.Log("��Ƽ���� ����� �ߵ��Ǿ����ϴ�.");
                break;
            case "������ ���":
                // ���� ���� ����
                Debug.Log("������ ����� �ߵ��Ǿ����ϴ�.");
                break;
            default:
                // �⺻ ����
                Debug.Log("�� �� ���� ����Դϴ�.");
                break;
        }

    }


}
