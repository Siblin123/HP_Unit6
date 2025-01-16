using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : baseStatus
{
    public NetworkAnimator NetworkAnimator;
    public Transform wallCheck;
    [Header("~Playerüũ")]
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
    [HideInInspector] public int randomMove_Direction;
    public float f_speed;
    protected float curSpeed;
    protected Vector2 view_dir;
    [Header("playerCheck")]
    public LayerMask find_layerMask;

    public override void Start()
    {
        base.Start();
        curSpeed = f_speed;
    }

    public virtual void Random_movement()//1.Ư�� ���� �ݺ�
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
                    transform.Translate(curSpeed * view_dir * Time.deltaTime);
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2:
                    view_dir = Vector2.left;
                    transform.Translate(curSpeed * view_dir * Time.deltaTime);
                    transform.localScale = new Vector3(1, 1, 1);
                    break;

            }
        }

    }

    public virtual bool WallCheck2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.up, 0.5f, ~wallLayer);
        Debug.DrawRay(wallCheck.position, Vector2.up * 0.5f, Color.red);
        if (hit.collider != null)
        {

            if (hit.transform.name.Contains("Plater"))
                print("find_PLayer");

            randomMove_Direction = Random.Range(0, 3);
            return true;
        }
        else
        {
            return false;
        }

      
    }

    [Header("�÷��̾� ã��")]
    public Transform findCehck;
    public float find_range;
    [Header("���� ������ �ִ� �÷��̾�")]
    public PlayerControl tart_player;
    [Header("�ڽ� ĳ��Ʈ�϶� ���")]
    public Vector2 find_size;
    public virtual void Find_Player() // ������ �þ� ����
    {
        Vector2 origin = (Vector2)findCehck.position;
        RaycastHit2D hit = Physics2D.BoxCast(origin, find_size, 0, Vector2.zero, find_range, find_layerMask);

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


    // �����Ϳ��� �ð�ȭ
    private void OnDrawGizmosSelected()
    {
        if (findCehck == null) return;

        Vector2 origin = (Vector2)findCehck.position;
        Gizmos.color = Color.blue;

        // �ڽ� ĳ��Ʈ ���� �ð�ȭ
        Gizmos.DrawWireCube(origin, find_size);
    }


}
