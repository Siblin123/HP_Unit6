using UnityEngine;

public class stair_Box : MonoBehaviour
{
    public stair main_stair;
    public PlatformEffector2D up_Ground;

    bool Is_playerIn;

    void Start()
    {
        main_stair = transform.parent.GetComponent<stair>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformEffector2D>() && collision.transform.position.y>transform.position.y)
        {
            up_Ground = collision.GetComponent<PlatformEffector2D>();
        }
       


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
            if (main_stair.Collider2D.isTrigger == false)
            {
                main_stair.Collider2D.isTrigger = true;
                collision.transform.GetComponent<PlayerControl>().movedir = Vector3.zero;
                up_Ground.surfaceArc = 180;
            }

        }



    }

}
