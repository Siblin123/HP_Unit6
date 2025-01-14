using UnityEngine;

public class shooter : Enemy
{
    //6. ���Ÿ� ���� , ���� �� �߽� , ���� �� ���ݹ����� ����  ���ݽ� �������� ����  2���� ���� 

    [Header("����ü ����")]
    public GameObject projectile_Obj;
    public float ShootTimer;
    public float ShootRange;
    float curShootTime;
    void Update()
    {
        
    }

    public void Shoot()
    {
        //�÷��̾���ġ�� ���� �¿� 

        if (ShootTimer <= curShootTime)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, find_range, find_layerMask);
            //������ �׷������� ���ھ�

            if (hit)
            {

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                //���� �ȿ� �÷��̾ �־ �÷��̾� ����
                if (distance <= ShootRange)
                    transform.position = hit.transform.position;
                else//�����ȿ� �÷��̾ ��� �÷��̾�������� �ִ�Ÿ� �̵��̵�
                {
                    Vector2 dir = hit.transform.position - transform.position;
                    dir = dir.normalized;

                    projectile_Obj.SetActive(true);
                    //�� �߽� �Ǿ����
                    

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
