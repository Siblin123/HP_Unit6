using Unity.Netcode;
using UnityEngine;

public class Fence : baseStatus 
{
    /*
     상호작용을 하면 문 열리는 애니메이션 실행 , 콜라이더 (꺼주기,켜주기)
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
            //닫기 애니메이션
            
            GetComponent<Collider2D>().isTrigger = false;
            isOpen = false;
        }
        else
        {
            //열기 애니메이션
            GetComponent<Collider2D>().isTrigger = true;
            isOpen = true;
        }
    }

}
