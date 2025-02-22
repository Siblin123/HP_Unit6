using UnityEngine;

public class Telpo : MonoBehaviour
{
    public PlayerControl player;
    public Transform telpo_Pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //만약에 자식이 없으면 부모가 telpo_Pos가 되고 자식이 있으면 자식이 telpo_Pos가 된다.
        if (transform.childCount!=0)
        {
            telpo_Pos = transform.GetChild(0);
        }
        else
        {
            telpo_Pos = transform.parent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(player!=null)
        {
            //만약에 플레이어가 위키를 누르면 플레이어의 위치를 텔포 위치로 이동
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                player.transform.position = telpo_Pos.position;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerControl>();
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}
