using UnityEngine;

public class reasearcher : Enemy
{
    //3.���� �������θ� ������� ����. 
    /*
     �÷��̾� �þߺ��� 2�� ���� �þ߸� ������ �ִ�. 
    �� ������ ���� ������ ������ ����ó�� Ư�� ������ �������� �����δ�. 
    �̶� �ӵ��� �÷��̾��� �ȴ� �ӵ��� �����ϴ�. 
    �÷��̾ �߰� �� �ȴ� �ӵ��� �����ϰ� ����´�. 
    �ش� ���ʹ� ���Ʒ��� �̵��� �Ұ��ϴ�. 
    ���� �þ߿��� �÷��̾ ����� �� �ռ� �ߴ� �ൿ�� �ݺ��Ѵ�.
     
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
            //�ִϸ��̼� ����
            //�̵��ӵ� ����
            //���̳� �÷��̾ ���������� ������ �Ѵ�
            view_dir = tart_player.transform.position - transform.position;
            view_dir.y = 0;
            view_dir.Normalize();
            transform.Translate(curSpeed * view_dir * Time.deltaTime);
            aaaaaaaaaaa=true;
        }
    }

}
