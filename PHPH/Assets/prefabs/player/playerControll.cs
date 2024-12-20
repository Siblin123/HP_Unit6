using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerControl : PlayerStatus
{
    public CinemachineVirtualCamera VirtualCamera;
    public AudioListener Listener;
    
    public Transform rayPos;

    public float rayDisance;
    public LayerMask wallLayer;



    public GameObject lastWall;



    //light_View
    public Light2D afternoon_light2D; // Light2D 컴포넌트
    public Light2D night_light2D; // Light2D 컴포넌트
    public Light2D curlight; // Light2D 컴포넌트
    public LayerMask enemyLayer; // 적이 포함된 레이어

    public override void Start()
    {
        base.Start();
       
        if (!IsOwner)
        {
            Listener.enabled = false;
            player_light.enabled = false;
        }
        else
        {
            //카메라 생성
            CinemachineVirtualCamera virtualCamera = Instantiate(VirtualCamera);
            virtualCamera.Follow = transform;
            Camera.main.GetComponent<CinemachineBrain>().IsLive(virtualCamera);

        }

    }

    public override void Update()
    {
        if (!IsOwner)
            return;
        base.Update();
        Light_Raycast();
        //list_View();
       // print("1");
    }



    //빛을 건너편이 보이게  하는 레이케스트
    private void Light_Raycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPos.position, rayDirection, rayDisance, wallLayer);

        if (hit.collider != null)
        {

            if (hit.collider.gameObject)
            {
                if (lastWall != null)
                    lastWall.GetComponent<ShadowCaster2D>().trimEdge = 0f;

                lastWall = hit.collider.gameObject;
                lastWall.GetComponent<ShadowCaster2D>().trimEdge = 1f;
            }
        }
        else
        {
            if (lastWall != null)
            {
                lastWall.GetComponent<ShadowCaster2D>().trimEdge = 0f;
                lastWall = null;    
            }
        }

        Debug.DrawRay(rayPos.position, rayDirection * rayDisance, Color.black);
       
    }

    void list_View()
    {
        if (!IsOwner) return;

        RaycastHit2D hit = Physics2D.Raycast(curlight.transform.position, Vector2.right, rayDisance, wallLayer);
        Debug.DrawRay(curlight.transform.position, Vector2.right* rayDisance);
    }

    public bool isOner_Game()
    {
        if(IsOwner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
