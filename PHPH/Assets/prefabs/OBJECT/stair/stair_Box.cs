using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class stair_Box : MonoBehaviour
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

        float VerticalInput = Input.GetAxisRaw("Vertical");
        if (VerticalInput > 0)
        {
            main_stair.Collider2D.isTrigger = false;
            up_Ground.rotationalOffset = 0;
        }
        else if(VerticalInput < 0)
        {
            up_Ground.surfaceArc = 0;
            main_stair.Collider2D.isTrigger = false;
        }



    }

    public void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            if (!collision.GetComponent<PlayerControl>().isOner_Game())
                return;

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
