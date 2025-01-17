using UnityEngine;

public class reasearcher : Enemy
{
    //3.가로 방향으로만 따라오는 몬스터. 
    /*
     플레이어 시야보다 2배 넓은 시야를 가지고 있다. 
    전 방향을 보고 있으며 메이플 몬스터처럼 특정 지역을 랜덤으로 움직인다. 
    이때 속도는 플레이어의 걷는 속도와 동일하다. 
    플레이어를 발견 시 걷는 속도와 동일하게 따라온다. 
    해당 몬스터는 위아래로 이동이 불가하다. 
    몬스터 시야에서 플레이어가 사라질 시 앞서 했던 행동을 반복한다.
     
     */
    public bool aaaaaaaaaaa;

    private void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();
    }


    public override void Random_movement()
    {
        if(!tart_player)
        {
            base.Random_movement();
            aaaaaaaaaaa = false;
        }
        else
        {
            //애니메이션 변경
            //이동속도 변경
            //벽이나 플레이어가 있을때까지 직진을 한다
            view_dir = tart_player.transform.position - transform.position;
            view_dir.y = 0;
            view_dir.Normalize();
            transform.Translate(curSpeed * view_dir * Time.deltaTime);
            aaaaaaaaaaa=true;
        }
    }

}
