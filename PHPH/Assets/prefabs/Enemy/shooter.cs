using UnityEngine;

public class shooter : Enemy
{
    //6. 원거리 공격 , 조준 후 발싸 , 조준 시 공격방향이 보임  공격시 움직이지 않음  2초후 공격 

    [Header("투사체 관련")]
    public GameObject projectile_Obj;
    public float ShootTimer;
    public float ShootRange;
    float curShootTime;
    void Update()
    {
        
    }

    public void Shoot()
    {
        //플레이어위치에 따른 좌우 

        if (ShootTimer <= curShootTime)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, find_range, find_layerMask);
            //범위를 그려줬으면 좋겠어

            if (hit)
            {

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                //범위 안에 플레이어가 있어서 플레이어 몸박
                if (distance <= ShootRange)
                    transform.position = hit.transform.position;
                else//범위안에 플레이어가 없어서 플레이어방향으로 최대거리 이동이동
                {
                    Vector2 dir = hit.transform.position - transform.position;
                    dir = dir.normalized;

                    projectile_Obj.SetActive(true);
                    //총 발싸 되어야함
                    

                }

            }

            ShootTimer = 0;
        }
        else
        {
            ShootTimer += Time.deltaTime;
        }

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, find_range);

    }
}
