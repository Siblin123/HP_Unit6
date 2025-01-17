using Unity.Netcode;
using UnityEngine;

public class shooter : Enemy
{
    //6. ���Ÿ� ���� , ���� �� �߽� , ���� �� ���ݹ����� ����  ���ݽ� �������� ����  2���� ���� 

    [Header("���� ��Ÿ��")]
    public Transform shooter_obj;
    public float ShootTimer;
    float curShootTime;

    [Header("���� ������ �� ���ݽð�")]
    Vector3 attackOffset= new Vector3(0,0.1f,0);
    public Vector2 dir;
    public float attackTime;
    [SerializeField] float curAttackTime;
    public LineRenderer lineRenderer;
    Vector3 linrendererPos;//�÷��̾ ������ ����� �������� �κ�
    [Header("��� �ִϸ��̼��� ������ �� �������� �÷��̾�")]
    public GameObject attack_tartget;

    public NetworkVariable<float> dirx = new NetworkVariable<float>();
    public NetworkVariable<float> diry = new NetworkVariable<float>();

    public override void Start()
    {
        lineRenderer= shooter_obj.GetComponent<LineRenderer>();
    }

    void Update()
    {
        Find_Player();
        shoot_ratcast();
    }

    public override void Find_Player()
    {
        //�÷��̾���ġ�� ���� �¿� 

        if (ShootTimer <= curShootTime)//������ �ϰ� ������ ��Ÿ��
        {
            Collider2D [] hit = Physics2D.OverlapCircleAll(transform.position, find_range, find_layerMask);

            if(hit.Length!=0)
            {
               
          
                for (int i=1; i<hit.Length; i++)
                {
                    if (hit.Length == 1)
                    {
                        tart_player = hit[0].transform.GetComponent<PlayerControl>();
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, hit[i].transform.position) > Vector2.Distance(transform.position, hit[i - 1].transform.position))
                        {
                            tart_player = hit[i-1].transform.GetComponent<PlayerControl>();
                           
                        }
                        else
                        {
                            tart_player = hit[i].transform.GetComponent<PlayerControl>();

                        }
                    }

                   



                }

            }
            else
            {
                print("NONONONO");
            }

            if (curAttackTime <= attackTime) //���� ����?�ð�
            {
                if (tart_player)
                {
                    dir = tart_player.transform.position - transform.position + attackOffset;
                    dir = dir.normalized;
                    tart_player = tart_player.gameObject.GetComponent<PlayerControl>();


                    if(IsServer)
                    {
                        dirx.Value = tart_player.transform.position.x;
                        diry.Value = tart_player.transform.position.y + attackOffset.y;
                    }

                    linrendererPos = Vector2.Lerp(linrendererPos, new Vector2(dirx.Value, diry.Value), 0.1f);
                    curAttackTime += Time.deltaTime;
                    print("5555555555555555");
                }
                else
                {
                    tart_player = null;

                    dir = Vector2.zero;
                    lineRenderer.SetPosition(1, linrendererPos);
                }
            }
            else
            {
                //�����ϱ� �ִϸ��̼����� �����ϰ� �ִϸ��̼ǿ��� Shoot() ����
                //NetworkAnimator.Animator.Play();
            }



        }
        else
        {
            curShootTime += Time.deltaTime;
        }

    }

    void shoot_ratcast()
    {
        if (dir != Vector2.zero && curAttackTime <= attackTime)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // z�ุ ȸ���ϵ��� ����
            shooter_obj.transform.rotation = Quaternion.Euler(0, 0, angle);

            RaycastHit2D rayHit = Physics2D.Raycast(shooter_obj.transform.position, dir, 100f, ~LayerMask.GetMask("enemy"));
            Debug.DrawRay(shooter_obj.transform.position, dir * 100f, Color.red);


            if (rayHit)
            {
                //�����ɽ�Ʈ ����
                //�ִϸ��̼����� ������ �ϸ� �ش� rayHit���ֿ��� �������� �ش�

                attack_tartget = rayHit.transform.gameObject;

            }
            else
            {
                attack_tartget = null;
            }



            //���� ������ ����
            if (lineRenderer != null)
            {
                // LineRenderer �������� ���� ����
                lineRenderer.positionCount = 2; // �� ������ ����
                lineRenderer.SetPosition(0, shooter_obj.transform.position); // ������
                if (tart_player != null)
                {
                    // ���̰� �浹�� �������� ���� �׸���
                    lineRenderer.SetPosition(1, linrendererPos);
                }


            }


            if (shooter_obj.transform.localPosition.x > dir.x)
            {
                transform.localScale = new Vector3(1, 1f, 1f);
                shooter_obj.transform.localScale = new Vector3(1, -1f, 1f);

            }
            else
            {
                transform.localScale = new Vector3(-1, 1f, 1f);
                shooter_obj.transform.localScale = new Vector3(-1, 1f, 1f);
            }


        }

    }


    //�ִϸ��̼����� ó���ϱ�
    public void Shoot()
    {
        if(attack_tartget!=null)
        {
            if(attack_tartget.GetComponent<PlayerControl>())
            {
                attack_tartget.GetComponent<PlayerControl>().Get_damage(damege, transform);
            }
        }
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, find_range);

    }
}
