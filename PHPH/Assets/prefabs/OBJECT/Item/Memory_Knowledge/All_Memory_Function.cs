using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;

public class All_Memory_Function : Item_Info
{

    public int max_Memory_Count; // �ִ�ġ

    public override void UseItem()
    {
        base.UseItem();

        int curCount = 0;
        foreach (string n in csTable.Instance.gameManager.player.have_Memory)
        {
            if (item_Name == n)
            {
                curCount++;
            }
        }

        if (curCount < max_Memory_Count)
        {

            csTable.Instance.gameManager.player.have_Memory.Add(item_Name);
            all_Function(item_Name);
            GetComponent<NetworkObject>().Despawn();

        }
        else
        {
            print("�̹� �ִ�ġ�� ȹ���߽��ϴ�.");
        }



    }


    public bool returnBool(string itemName)
    {
        switch (itemName)
        {

            case "������ ���":
                // ���� ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;
                

            case "�Ѿ��� ���":
                // �Ѿ� ���� ����
                Debug.Log("�Ѿ��� ���� �� �ִ�. �ִ�ġ=1");
                return true;

            case "���� ���":
                // �� ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;

            case "���� �������� ���":
                // ������ ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;

            case "���� ���":
                // �� ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;

            case "�������� ���":
                // ������ ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;

            case "����Ű�� ���":
                // ����Ű ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;


            case "������ ���":
                // ���� ���� ����
                Debug.Log("��ø���� ���� �� ����. �ִ�ġ=1");
                return true;

            case "��ȯ���� ���":
                // ��ȯ�� ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;


            case "�Ź��� ���":
                // �Ź� ���� ����
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                return true;


            default:
                return false;
        }
    }


    public void retrunFunction(string itemName)
    {
        switch (itemName)
        {
            case "�������� ���":
                // ������ ���� ���� ax_memory_Value ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ��Ÿ���� �پ���.���� ��Ÿ��-0.3��");
                csTable.Instance.gameManager.player.ax_memory_Value += 0.3f;
                break;

            case "������ ���":
                // ���� ���� ����  pick_memory_Value ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ��Ÿ���� �پ���. ��� ��Ÿ��-0.3��");
                csTable.Instance.gameManager.player.pick_memory_Value += 0.3f;
                break;

            case "����� ���":
                // ��� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ����ִ� ������ ĭ �Ϻθ� ����� �� �ְ� �ȴ�.  �ִ�ġ=6");
                break;

      
            case "�������� ���":
                // ������ ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ������ �Ǹ� ������ �����Ѵ�.    ���� �Ǹ� ����+1 �ִ�ġ=6");
                break;

            case "���ݼ��� ���":
                // ���ݼ� ���� ����  longAttackDamegeUp_Value����
                Debug.Log("��ø���� ���� �� ������, ���� ������ �ɷ�ġ�� �����Ѵ�. ���Ÿ� �����+0.3");
                csTable.Instance.gameManager.player.longAttackDamegeUp_Value += 0.3f;
                break;

            case "���� ������ ���":
                // ���� ���� ���� ����  shortAttackDamageUp_Value����
                Debug.Log("��ø���� ���� �� ������, ���� ������ �ɷ�ġ�� �����Ѵ�. ���� ������+0.3");
                csTable.Instance.gameManager.player.shortAttackDamageUp_Value += 0.3f;
                break;
        }

    }

    public void Gotget_Function(string itemName)
    {
        switch(itemName)
        {
            case "�ɸ����� ���":
                // �ɸ��� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� ä������ ���� Ȯ���� ����� �߰��� �� �ִ�.");
                break;

            case "���°��� ���":
                // ���°� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� ���ÿ��� ���� Ȯ���� �� ���� �� �ִ�.");
                break;

            case "��Ƽ���� ���":
                // ��Ƽ�� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� �������� ���� Ȯ���� ���̾Ƹ�带 Ķ �� �ִ�.");
                break;

            case "������ ���":
                // ���� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� ���͸� ������ ������ Ȯ���� ����ġ 1�� ��ᰡ ������, �ſ� ���� Ȯ���� ����ġ�� 100 �̸��� �������� ���´�.");
                break;
        }
    }
    

    public void all_Function(string itemName)
    {

        switch (itemName)
        {
            case "������ ���":
                // ���� ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;

            case "�Ѿ��� ���":
                // �Ѿ� ���� ����
                returnBool(itemName);
                Debug.Log("�Ѿ��� ���� �� �ִ�. �ִ�ġ=1");
                break;

            case "���� ���":
                // �� ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;

            case "���� �������� ���":
                // ������ ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;

            case "���� ���":
                // �� ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;

            case "�������� ���":
                // ������ ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;

            case "����Ű�� ���":
                // ����Ű ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;


            case "������ ���":
                // ���� ���� ����
                returnBool(itemName);
                Debug.Log("��ø���� ���� �� ����. �ִ�ġ=1");
                break;

            case "��ȯ���� ���":
                // ��ȯ�� ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;


            case "�Ź��� ���":
                // �Ź� ���� ����
                returnBool(itemName);
                Debug.Log("���� �� �ִ�. �ִ�ġ=1");
                break;

            case "�������� ���":
                // ������ ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ��Ÿ���� �پ���.���� ��Ÿ��-0.3��");
                retrunFunction(itemName);
                break;

            case "������ ���":
                // ���� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ��Ÿ���� �پ���. ��� ��Ÿ��-0.3��");
                retrunFunction(itemName);
                break;

            case "����� ���":
                // ��� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ����ִ� ������ ĭ �Ϻθ� ����� �� �ְ� �ȴ�.  �ִ�ġ=6");
                retrunFunction(itemName);
                break;


            case "�������� ���":
                // ������ ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ ������ �Ǹ� ������ �����Ѵ�.    ���� �Ǹ� ����+1 �ִ�ġ=6");
                retrunFunction(itemName);
                break;

            case "���ݼ��� ���":
                // ���ݼ� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ �ɷ�ġ�� �����Ѵ�. ���Ÿ� �����+0.3");
                retrunFunction(itemName);
                break;

            case "���� ������ ���":
                // ���� ���� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� ������ �ɷ�ġ�� �����Ѵ�. ���� ������+0.3");
                retrunFunction(itemName);
                break;

            case "�ɸ����� ���":
                // �ɸ��� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� ä������ ���� Ȯ���� ����� �߰��� �� �ִ�.");
                Gotget_Function(itemName);
                break;

            case "���°��� ���":
                // ���°� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� ���ÿ��� ���� Ȯ���� �� ���� �� �ִ�.");
                Gotget_Function(itemName);
                break;

            case "��Ƽ���� ���":
                // ��Ƽ�� ���� ����
                Debug.Log("��ø���� ���� �� ������, ���� �� �������� ���� Ȯ���� ���̾Ƹ�带 Ķ �� �ִ�.");
                Gotget_Function(itemName);
                break;

            case "������ ���":
                // ���� ���� ����
                Gotget_Function(itemName);
                Debug.Log("��ø���� ���� �� ������, ���� �� ���͸� ������ ������ Ȯ���� ����ġ 1�� ��ᰡ ������, �ſ� ���� Ȯ���� ����ġ�� 100 �̸��� �������� ���´�.");
                break;

            default:
                // �⺻ ����
                Debug.Log("�� �� ���� ����Դϴ�.");
                break;
        }
    }




}
