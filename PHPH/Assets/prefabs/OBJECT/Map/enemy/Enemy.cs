using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : baseStatus
{
    public Transform wallCheck;
    public LayerMask wallLayer;


    enum Enemy_Type
    {
        Random_movement,//���� �̵�
        chase,//�÷��̾� ����
        attack
    }

    Enemy_Type enemy_Type;


    //����: Ư�� ���� �ݺ�,  �÷��̾� ���� , 

    [Header("�����̴� �ð�")]
    [SerializeField] float randomMove_Num;
    [SerializeField] float randomMove_Time;
    [Header("0 ���� 1 ���� 2 ������")]
    [SerializeField] int randomMove_Direction;
    [SerializeField] float speed;
    Vector2 view_dir;
    public LayerMask layerMask;
    public void Random_movement()//1.Ư�� ���� �ݺ�
    {

        if (randomMove_Time <= randomMove_Num)
        {
            randomMove_Time = Random.Range(1, 3);
            randomMove_Direction = Random.Range(0, 3);
            randomMove_Num = 0;
        }
        else
        {
            randomMove_Num += Time.deltaTime;
            switch (randomMove_Direction)
            {
               
                case 0:
                    //�������� �ʰ� ����
                    break;
                case 1:
                    view_dir = Vector2.right;
                    transform.Translate(view_dir * Time.deltaTime);
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2:
                    view_dir = Vector2.left;
                    transform.Translate(view_dir * Time.deltaTime);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;

            }
        }

    }

    public void WallCheck2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.up, 0.5f, ~wallLayer);
        Debug.DrawRay(wallCheck.position, Vector2.up * 0.5f, Color.red);
        if (hit.collider != null)
        {

            if (hit.transform.name.Contains("Plater"))
                print("find_PLayer");

            randomMove_Direction = Random.Range(0, 3); 
        }
    }

    public float find_range;
    public PlayerControl tart_player;
    public void Find_Player()//������ �þ� ����
    {

        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, view_dir, find_range, ~layerMask);
        Debug.DrawRay(wallCheck.position, view_dir * find_range, Color.blue);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                tart_player = hit.collider.GetComponent<PlayerControl>();
            }
        }
        else
        {
            tart_player = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerControl>().Get_damage(damege,transform);
            
        }
    }
}
