using UnityEngine;

public class dreaming_girl : Enemy
{
    //5. ã�� �����ȿ� �÷��̾������� �÷��̾� �������� �����̵�
    // �����̵� �������� �÷��̾� ������ ����
    [Header("���� �̵��Ÿ�")]
    public float teleport_Range;
    public float teleportTime;
    [SerializeField]float curTeleportTime;
    private void Update()
    {
        Teleport();
    }

    public void Teleport()
    {
        if (teleportTime <= curTeleportTime)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, teleport_Range, find_layerMask);
            //������ �׷������� ���ھ�

            if (hit)
            {
                transform.position = hit.transform.position;
            }
            else
            {
                int num = Random.Range(-1, 1);
                Vector3 newpos = new Vector2(transform.position.x + num, transform.position.y);
                transform.position=Vector2.MoveTowards(transform.position, newpos, 1f);
            }

            curTeleportTime = 0;
        }
        else
        {
            curTeleportTime += Time.deltaTime;
        }

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, teleport_Range);

    }


}
