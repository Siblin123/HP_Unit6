using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : baseStatus
{
    enum Enemy_Type
    {
        Random_movement,//랜덤 이동
        chase,//플레이어 추적
        attack
    }

    Enemy_Type enemy_Type;

    //1.특정 구역 반복

    //2.시각만 존재하는 몬스터. 들키면 직직으로 빠르게 달려온다.

    //3.가로 방향으로만 따라오는 몬스터. 

    //4. 날아다니면서 플레이어 방향으로 이동

    //5. 찾는 범위안에 플레이어있으면 플레이어 방향으로 순간이동 , 순간이동 범위내에 플레이어 있으면 몸박

    //6. 원거리 공격 , 조준 후 발싸 , 조준 시 공격방향이 보임  공격시 움직이지 않음  2초후 공격 

    //공통: 특정 구역 반복,  플레이어 추적 , 


    float randomMove_Num;
    float randomMove_Time;
    int randomMove_Direction;


    public void Random_movement()
    {
      
        if(randomMove_Time<= randomMove_Num)
        {
            randomMove_Time = Random.Range(1, 3);
            randomMove_Direction = Random.Range(0, 3);
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
                    transform.Translate(Vector3.back * Time.deltaTime);
                    break;
                case 2:
                    transform.Translate(Vector3.left * Time.deltaTime);
                    break;

            }
        }

    }

    public void WallCheck2D()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(transform.position, Vector2.up * 0.5f, Color.red);
        if (hit.collider != null)
        {
            randomMove_Direction = 0;
        }
    }
    

}
