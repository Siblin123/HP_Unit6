using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class runner : Enemy
{
    //2.�ð��� �����ϴ� ����. ��Ű�� �������� ������ �޷��´�. 
    public float runSpeed;

    //����
    public float runTime;
    public bool isRunCorutine = false;
    // Update is called once per frame
    void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();

        //�÷��̾ Ȯ���� �Ǹ� ���̳� �÷��̾���� �����Ѵ�

    }

    public override void Random_movement()
    {
        if (tart_player == null && !isRunCorutine)
        {
            base.Random_movement();

        }
        else
        {
            //�ִϸ��̼� ����
            //�̵��ӵ� ����
            //���̳� �÷��̾ ���������� ������ �Ѵ�

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

        yield return new WaitForSeconds(1f);//<- ������ ���� �÷��̾� ã����� ���ð�
        isRunCorutine = false;
    }




}
