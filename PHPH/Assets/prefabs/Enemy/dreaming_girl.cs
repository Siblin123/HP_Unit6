using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class dreaming_girl : Enemy
{
    //5. ã�� �����ȿ� �÷��̾������� �÷��̾� �������� �����̵�
    // �����̵� �������� �÷��̾� ������ ����

    [Header("���� �̵��Ÿ�")]
    public float teleport_Range;
    public float teleportTime;
    [SerializeField]float curTeleportTime;
    [Header("Ground ����")]
    public LayerMask find_Ground;

    [Header("�����̵����� �̸� ���������� ������Ʈ")]
    public GameObject ready_Move_Obj;
    public bool isCorutineRun;
    private void Update()
    {
        Find_Player();
    }

    public override void Find_Player()
    {
        if (teleportTime <= curTeleportTime)
        {
            Collider2D hit = Physics2D.OverlapCircle( transform.position, find_range, find_layerMask);
            //������ �׷������� ���ھ�

            if (hit)
            {

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                //���� �ȿ� �÷��̾ �־ �÷��̾� ����
                if (distance <= teleport_Range)
                    transform.position = hit.transform.position;
                else//�����ȿ� �÷��̾ ��� �÷��̾�������� �ִ�Ÿ� �̵��̵�
                {
                    Vector2 dir = hit.transform.position - transform.position;
                    dir= dir.normalized;

                    ready_Move_Obj.SetActive(true);
                    ready_Move_Obj.transform.position = transform.position + (Vector3)(dir * teleport_Range);
                    //1.�÷��̾� ���� �ִ� �Ÿ��� �߷��� �ִ� ������Ʈ ��ȯ
                    if(!isCorutineRun)
                        StartCoroutine(Teleport_PosCheck_Obj());
                 

                }

            }

            curTeleportTime = 0;
        }
        else
        {
            curTeleportTime += Time.deltaTime;
        }

    }


    IEnumerator Teleport_PosCheck_Obj()
    {

        isCorutineRun = true;
        while (true)
        {
        

            //������Ʈ�� �ָ������� �����̵��� �����ʴ´�
            float distance = Vector2.Distance(transform.position, ready_Move_Obj.transform.position);
            if (distance > teleport_Range)
            {
                ready_Move_Obj.SetActive(false);
                break;
            }

            yield return null;

            if (ready_Move_Obj.GetComponent<Rigidbody2D>().linearVelocityY == 0)
            {
                transform.position = ready_Move_Obj.transform.position;
                ready_Move_Obj.SetActive(false);
                break;
            }
        }

        isCorutineRun = false;
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, find_range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, teleport_Range);
    }


}
