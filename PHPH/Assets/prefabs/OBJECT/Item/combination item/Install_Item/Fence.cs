using Unity.Netcode;
using UnityEngine;

public class Fence : baseStatus 
{
    /*
     ��ȣ�ۿ��� �ϸ� �� ������ �ִϸ��̼� ���� , �ݶ��̴� (���ֱ�,���ֱ�)
     */

    public override void interact()
    {
        base.interact();
        on_Off_Door();
    }

    [SerializeField] bool isOpen;
    public void on_Off_Door()
    {
        if(isOpen)
        {
            //�ݱ� �ִϸ��̼�
            
            GetComponent<Collider2D>().isTrigger = false;
            isOpen = false;
        }
        else
        {
            //���� �ִϸ��̼�
            GetComponent<Collider2D>().isTrigger = true;
            isOpen = true;
        }
    }

}
