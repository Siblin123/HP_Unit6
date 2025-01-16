using UnityEngine;

public class shooter : Enemy
{
    //6. 원거리 공격 , 조준 후 발싸 , 조준 시 공격방향이 보임  공격시 움직이지 않음  2초후 공격 

    [Header("공격 쿨타임")]
    public Transform shooter_obj;
    public float ShootTimer;
    float curShootTime;

    [Header("공격 딜레이 및 공격시간")]
    Vector3 attackOffset= new Vector3(0,0.1f,0);
    public Vector2 dir;
    public float attackTime;
    [SerializeField] float curAttackTime;
    public LineRenderer lineRenderer;
    Vector3 linrendererPos;//플레이어가 범위를 벗어나면 멈춰지는 부분
    [Header("쏘는 애니메이션이 나왔을 때 때려지는 플레이어")]
    public GameObject attack_tartget;
    public override void Start()
    {
        lineRenderer= shooter_obj.GetComponent<LineRenderer>();
    }

    void Update()
    {
        Find_Player();
        shoot_ratcast();
    }

    public override void Find_Player()
    {
        //플레이어위치에 따른 좌우 

        if (ShootTimer <= curShootTime)//공격을 하고 나서의 쿨타임
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, find_range, find_layerMask);

            if (curAttackTime <= attackTime) //공격 충전?시간
            {
                if (hit)
                {
                    dir = hit.transform.position - transform.position + attackOffset;
                    dir = dir.normalized;
                    tart_player = hit.gameObject.GetComponent<PlayerControl>();
                    linrendererPos = hit.transform.position + attackOffset;

                    curAttackTime += Time.deltaTime;
                }
                else
                {
                    tart_player = null;

                    dir = Vector2.zero;
                    lineRenderer.SetPosition(1, linrendererPos);
                }
            }
            else
            {
                //공격하기 애니메이션으로 공격하고 애니메이션에서 Shoot() 실행
                //NetworkAnimator.Animator.Play();
            }



        }
        else
        {
            curShootTime += Time.deltaTime;
        }

    }

    void shoot_ratcast()
    {
        if (dir != Vector2.zero && curAttackTime <= attackTime)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // z축만 회전하도록 설정
            shooter_obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            RaycastHit2D rayHit = Physics2D.Raycast(shooter_obj.transform.position, dir, 100f, ~LayerMask.GetMask("enemy"));
            Debug.DrawRay(shooter_obj.transform.position, dir * 100f, Color.red);


            if (rayHit)
            {
                //레이케스트 구간
                //애니메이션으로 공격을 하면 해당 rayHit유닛에게 데미지를 준다

                attack_tartget = rayHit.transform.gameObject;

            }
            else
            {
                attack_tartget = null;
            }



            //라인 랜더버 구간
            if (lineRenderer != null)
            {
                // LineRenderer 시작점과 끝점 설정
                lineRenderer.positionCount = 2; // 두 점으로 구성
                lineRenderer.SetPosition(0, shooter_obj.transform.position); // 시작점
                if (tart_player != null)
                {
                    // 레이가 충돌한 지점까지 라인 그리기
                    lineRenderer.SetPosition(1, linrendererPos);
                }


            }


            if (shooter_obj.transform.localPosition.x > dir.x)
            {
                transform.localScale = new Vector3(1, 1f, 1f);
                shooter_obj.transform.localScale = new Vector3(1, -1f, 1f);

            }
            else
            {
                transform.localScale = new Vector3(-1, 1f, 1f);
                shooter_obj.transform.localScale = new Vector3(-1, 1f, 1f);
            }


        }

    }


    //애니메이션으로 처리하기
    public void Shoot()
    {
        if(attack_tartget!=null)
        {
            if(attack_tartget.GetComponent<PlayerControl>())
            {
                attack_tartget.GetComponent<PlayerControl>().Get_damage(damege, transform);
            }
        }
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, find_range);

    }
}
