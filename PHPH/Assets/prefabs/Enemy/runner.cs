using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class runner : Enemy
{
    //2.시각만 존재하는 몬스터. 들키면 직직으로 빠르게 달려온다. 
    public float runSpeed;

    //돌진
    public float runTime;
    public bool isRunCorutine = false;
    // Update is called once per frame
    void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();

        //플레이어가 확인이 되면 벽이나 플레이어까지 돌진한다

    }

    public override void Random_movement()
    {
        if (tart_player == null && !isRunCorutine)
        {
            base.Random_movement();

        }
        else
        {
            //애니메이션 변경
            //이동속도 변경
            //벽이나 플레이어가 있을때까지 직진을 한다

            if(!isRunCorutine)
                StartCoroutine(Run());
            
        }


    }

    IEnumerator Run()
    {
        isRunCorutine = true;
        float timer=0;
        while (true)
        {
            timer += Time.deltaTime;
            transform.Translate(runSpeed * view_dir * Time.deltaTime);
            yield return null;

            if(timer >= runTime)
                break;

            if (WallCheck2D())
            {
                view_dir.Set(-view_dir.x, view_dir.y);
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                break;
            }
        }

        yield return new WaitForSeconds(1f);//<- 돌진후 다음 플레이어 찾기까지 대기시간
        isRunCorutine = false;
    }




}
