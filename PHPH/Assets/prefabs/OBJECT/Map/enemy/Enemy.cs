using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : baseStatus
{
    enum Enemy_Type
    {
        Random_movement,//���� �̵�
        chase,//�÷��̾� ����
        attack
    }

    Enemy_Type enemy_Type;

    //1.Ư�� ���� �ݺ�

    //2.�ð��� �����ϴ� ����. ��Ű�� �������� ������ �޷��´�.

    //3.���� �������θ� ������� ����. 

    //4. ���ƴٴϸ鼭 �÷��̾� �������� �̵�

    //5. ã�� �����ȿ� �÷��̾������� �÷��̾� �������� �����̵� , �����̵� �������� �÷��̾� ������ ����

    //6. ���Ÿ� ���� , ���� �� �߽� , ���� �� ���ݹ����� ����  ���ݽ� �������� ����  2���� ���� 

    //����: Ư�� ���� �ݺ�,  �÷��̾� ���� , 


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
                    //�������� �ʰ� ����
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
