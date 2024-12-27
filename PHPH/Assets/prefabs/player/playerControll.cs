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
    public GameObject lastWallL;
    public GameObject lastWallR;



    //light_View
    public Light2D afternoon_light2D; // Light2D ������Ʈ
    public Light2D night_light2D; // Light2D ������Ʈ
    public Light2D curlight; // Light2D ������Ʈ
    public LayerMask enemyLayer; // ���� ���Ե� ���̾�

    //interact()
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
            //ī�޶� ����
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



    //���� �ǳ����� ���̰�  �ϴ� �����ɽ�Ʈ
    private void Light_Raycast()
    {
       



        if(csTable.Instance.gameManager.is_afterNoonNight==1)//�㿡�� �۵�
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

    public void interact_Object()
    {
        // ���̸� ���� �� �տ� �ִ� ������Ʈ�� ã����
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1, interaction_Layer);

        // ���̰� ������Ʈ�� �¾Ҵ��� Ȯ��
        if (hit.collider != null)
        {
            // ��ȣ�ۿ� ������ ������Ʈ���� Ȯ��
            interactable = hit.collider.GetComponent<interaction>();
            if (interactable != null)
            {
                // E Ű�� ������ ��ȣ�ۿ� ����
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.interact();
                }
            }
        }

        // ����׿� ���� �׸���
        Debug.DrawRay(transform.position, rayDirection * rayDisance, Color.red);
    }

}
