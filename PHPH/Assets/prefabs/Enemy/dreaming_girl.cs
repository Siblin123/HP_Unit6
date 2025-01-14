using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class dreaming_girl : Enemy
{
    //5. 찾는 범위안에 플레이어있으면 플레이어 방향으로 순간이동
    // 순간이동 범위내에 플레이어 있으면 몸박

    [Header("텔포 이동거리")]
    public float teleport_Range;
    public float teleportTime;
    [SerializeField]float curTeleportTime;
    [Header("Ground 선택")]
    public LayerMask find_Ground;

    [Header("순간이동전에 미리 움직여보는 오브젝트")]
    public GameObject ready_Move_Obj;
    public bool isCorutineRun;
    private void Update()
    {
        Find_Player();
    }

    public override void Find_Player()
    {
        if (teleportTime <= curTeleportTime)
        {
            Collider2D hit = Physics2D.OverlapCircle( transform.position, find_range, find_layerMask);
            //범위를 그려줬으면 좋겠어

            if (hit)
            {

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                //범위 안에 플레이어가 있어서 플레이어 몸박
                if (distance <= teleport_Range)
                    transform.position = hit.transform.position;
                else//범위안에 플레이어가 없어서 플레이어방향으로 최대거리 이동이동
                {
                    Vector2 dir = hit.transform.position - transform.position;
                    dir= dir.normalized;

                    ready_Move_Obj.SetActive(true);
                    ready_Move_Obj.transform.position = transform.position + (Vector3)(dir * teleport_Range);
                    //1.플레이어 방향 최대 거리로 중력이 있는 오브젝트 소환
                    if(!isCorutineRun)
                        StartCoroutine(Teleport_PosCheck_Obj());
                 

                }

            }

            curTeleportTime = 0;
        }
        else
        {
            curTeleportTime += Time.deltaTime;
        }

    }


    IEnumerator Teleport_PosCheck_Obj()
    {

        isCorutineRun = true;
        while (true)
        {
        

            //오브젝트가 멀리있으면 순간이동을 하지않는다
            float distance = Vector2.Distance(transform.position, ready_Move_Obj.transform.position);
            if (distance > teleport_Range)
            {
                ready_Move_Obj.SetActive(false);
                break;
            }

            yield return null;

            if (ready_Move_Obj.GetComponent<Rigidbody2D>().linearVelocityY == 0)
            {
                transform.position = ready_Move_Obj.transform.position;
                ready_Move_Obj.SetActive(false);
                break;
            }
        }

        isCorutineRun = false;
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, find_range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, teleport_Range);
    }


}
