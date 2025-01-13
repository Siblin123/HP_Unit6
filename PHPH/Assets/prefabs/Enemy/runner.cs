using UnityEngine;
using UnityEngine.Rendering.Universal;

public class runner : Enemy
{
    //2.시각만 존재하는 몬스터. 들키면 직직으로 빠르게 달려온다. 
    public float runSpeed;

    //돌진
    bool isRun = false;
    public float runTime;
    public float curRunTime = 0;
    // Update is called once per frame
    void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();

        //플레이어가 확인이 되면 벽이나 플레이어까지 돌진한다
        Run();
    }

    public override void Random_movement()
    {
        if (tart_player == null && isRun==false)
        {
            base.Random_movement();
          
        }
        else
        {
            //애니메이션 변경
            //이동속도 변경
            //벽이나 플레이어가 있을때까지 직진을 한다
            isRun = true;

        }
      

    }

    public override bool WallCheck2D()
    {
      
        if (isRun && base.WallCheck2D())
        {
            isRun = false;
            randomMove_Direction = 0;
            curRunTime = 0;
        }
        else
        {
            base.WallCheck2D();
        }

        return false;
    }

    public void Run()
    {
        if (isRun)
        {
            transform.Translate(runSpeed * view_dir * Time.deltaTime);

            if (runTime >= curRunTime)
            {
                curRunTime += Time.deltaTime;
            }
            else
            {
                isRun = false;
                curRunTime = 0;
            }

        }
    }
}
