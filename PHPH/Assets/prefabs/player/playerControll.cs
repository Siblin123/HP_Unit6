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
    public Light2D curlight; // Light2D 컴포넌트
    public LayerMask enemyLayer; // 적이 포함된 레이어

    //interact()
    public Vector3 interactableOffset;
    public interaction interactable;
    public LayerMask interaction_Layer;
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
            //플레이어 위치 초기화
            transform.position=csTable.Instance.unitSopn_Pos.position;
            //카메라 생성
            CinemachineVirtualCamera virtualCamera = Instantiate(VirtualCamera);
            virtualCamera.Follow = transform;
            Camera.main.GetComponent<CinemachineBrain>().IsLive(virtualCamera);
            csTable.Instance.gameManager.player = this;
        }

    }

    public override void Update()
    {
        if (!IsOwner)
            return;
        base.Update();
        Light_Raycast();
        interact_Object();
        //list_View();
        // print("1");
    }



    //빛을 건너편이 보이게  하는 레이케스트
    private void Light_Raycast()
    {
        if(csTable.Instance.gameManager.is_afterNoonNight.Value ==1)//밤에만 작동
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
        }



      //  Debug.DrawRay(rayPos.position, rayDirection * rayDisance, Color.black);
       
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

    public void interact_Object()
    {
        // 레이를 쏴서 내 앞에 있는 오브젝트를 찾아줌
        RaycastHit2D hit = Physics2D.Raycast(transform.position + interactableOffset, rayDirection, 1, interaction_Layer);

        // 레이가 오브젝트에 맞았는지 확인
        if (hit.collider != null)
        {
            // 상호작용 가능한 오브젝트인지 확인
            interactable = hit.collider.GetComponent<interaction>();
            if (interactable != null)
            {
                // E 키를 누르면 상호작용 실행
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.interact();
                }
            }
        }
        else
        {
            interactable = null;
        }

        // 디버그용 레이 그리기
         Debug.DrawRay(transform.position + interactableOffset, rayDirection * rayDisance, Color.red);
    }

}
