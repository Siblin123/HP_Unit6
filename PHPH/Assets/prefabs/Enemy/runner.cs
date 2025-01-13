using UnityEngine;
using UnityEngine.Rendering.Universal;

public class runner : Enemy
{
    //2.�ð��� �����ϴ� ����. ��Ű�� �������� ������ �޷��´�. 
    public float runSpeed;

    //����
    bool isRun = false;
    public float runTime;
    public float curRunTime = 0;
    // Update is called once per frame
    void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();

        //�÷��̾ Ȯ���� �Ǹ� ���̳� �÷��̾���� �����Ѵ�
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
            //�ִϸ��̼� ����
            //�̵��ӵ� ����
            //���̳� �÷��̾ ���������� ������ �Ѵ�
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
