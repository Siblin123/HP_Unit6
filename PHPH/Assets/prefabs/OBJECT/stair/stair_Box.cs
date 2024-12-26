using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class stair_Box : NetworkBehaviour
{
    public LayerMask ground;
    public stair main_stair;
    public PlatformEffector2D up_Ground;



    bool Is_playerIn;

    void Start()
    {
        main_stair = transform.parent.GetComponent<stair>();
       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            up_Ground = collision.transform.GetComponent<PlatformEffector2D>();
        }

        if (collision.GetComponent<NetworkBehaviour>())
            if (!collision.GetComponent<NetworkBehaviour>().IsOwner)
                return;





        if (collision.transform.CompareTag("Player"))
        {
            print("계단에 입장");

            float VerticalInput = Input.GetAxisRaw("Vertical");
            if (VerticalInput > 0)
            {
                main_stair.Collider2D.isTrigger = false;
                up_Ground.rotationalOffset = 0;
            }
            else if (VerticalInput < 0)
            {
                up_Ground.surfaceArc = 0;
                main_stair.Collider2D.isTrigger = false;
            }

        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<NetworkBehaviour>())
            if (!collision.GetComponent<NetworkBehaviour>().IsOwner)
                return;


        if (collision.transform.CompareTag("Player"))
        {
            print("계단에 탈출");
            if (main_stair.Collider2D.isTrigger == false)
            {
                main_stair.Collider2D.isTrigger = true;
                up_Ground.surfaceArc = 180;
            }
            collision.transform.GetComponent<PlayerControl>().movedir = Vector3.zero;
            collision.transform.GetComponent<Rigidbody2D>().linearVelocityY =0;
        }



    }

}
