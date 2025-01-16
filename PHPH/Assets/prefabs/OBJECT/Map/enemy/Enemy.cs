using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : baseStatus
{
    public NetworkAnimator NetworkAnimator;
    public Transform wallCheck;
    [Header("~Player체크")]
    public LayerMask wallLayer;


    enum Enemy_Type
    {
        Random_movement,//랜덤 이동
        chase,//플레이어 추적
        attack
    }

    Enemy_Type enemy_Type;


    //공통: 특정 구역 반복,  플레이어 추적 , 

    [Header("움직이는 시간")]
    [SerializeField] float randomMove_Num;
    [SerializeField] float randomMove_Time;
    [Header("0 정지 1 왼쪽 2 오른쪽")]
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

    public virtual void Random_movement()//1.특정 구역 반복
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
                    //움직이지 않고 정지
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

    [Header("플레이어 찾기")]
    public Transform findCehck;
    public float find_range;
    [Header("공격 범위에 있는 플레이어")]
    public PlayerControl tart_player;
    [Header("박스 캐스트일때 사용")]
    public Vector2 find_size;
    public virtual void Find_Player() // 유닛의 시야 범위
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


    // 에디터에서 시각화
    private void OnDrawGizmosSelected()
    {
        if (findCehck == null) return;

        Vector2 origin = (Vector2)findCehck.position;
        Gizmos.color = Color.blue;

        // 박스 캐스트 영역 시각화
        Gizmos.DrawWireCube(origin, find_size);
    }


}
