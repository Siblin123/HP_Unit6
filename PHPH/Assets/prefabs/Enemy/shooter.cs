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
    
    [Header("��� �ִϸ��̼��� ������ �� �������� �÷��̾�")]
    public GameObject attack_tartget;

    Vector2 attackdir;
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




                    curAttackTime += Time.deltaTime;
                    print("5555555555555555");
                }
                else
                {
                    tart_player = null;

                    dir = Vector2.zero;
                    lineRenderer.SetPosition(1, shooter_obj.transform.position);
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
            


            if (IsServer)
            {
                // Raycast�� ���� ��� ����
                if (rayHit.collider != null)
                {
                    NetworkObject targetNetObj = rayHit.collider.GetComponent<NetworkObject>();

                    if (targetNetObj != null)
                    {
                        // Ŭ���̾�Ʈ���� ���� ��� ���� ����ȭ
                        NotifyAttackTargetClientRpc(targetNetObj.NetworkObjectId);
                    }
                }
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
                    lineRenderer.SetPosition(1, attack_tartget.transform.position);
                    Debug.DrawRay(shooter_obj.transform.position, (attack_tartget.transform.position  - shooter_obj.transform.position ).normalized* 100f, Color.red);
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

    [ClientRpc]
    private void NotifyAttackTargetClientRpc(ulong targetNetworkId)
    {
        print("zzzzzzzzz");
        // ��� Ŭ���̾�Ʈ�� ������ ��� ����
        NetworkObject target = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetNetworkId];
        if (target != null)
        {
            attack_tartget = target.gameObject;
            Debug.Log("Attack Target Synced: " + target.name);
            // ���ϴ� ���� ó��
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
