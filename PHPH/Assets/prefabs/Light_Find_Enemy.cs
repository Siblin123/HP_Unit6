using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;

public class Light_Find_Enemy : NetworkBehaviour
{
    public Light2D afternoon_light2D; // Light2D ������Ʈ
    public Light2D night_light2D; // Light2D ������Ʈ
    public Light2D light2D; // Light2D ������Ʈ
    public LayerMask enemyLayer; // ���� ���Ե� ���̾�
    public float rayLength = 10f; // ������ ����
    public int rayCount = 36; // �ݿ� ����� ���� ����
    public float outerAngle = 180f; // ���� �ܺ� ����

    public List<Enemy> checkEnemy; // �߰ߵ� ������ ������ ����Ʈ
    public bool is_E;

    private void Start()
    {
        if (!IsOwner)
            return;
      
    }


    private void OnDisable()
    {
        if (gameObject.name.Contains("night_light2D"))
            StopCoroutine("find_Enemy");
    }

    private void OnEnable()
    {

        if(gameObject.name.Contains("night_light2D"))
            StartCoroutine("find_Enemy");
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Change_Light(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Change_Light(false);
        }
    }
    IEnumerator find_Enemy()
    {
        //print("startCo");
        is_E = false;
        Vector2 lightPosition = light2D.transform.position;
        float outerRadius = light2D.pointLightOuterRadius;
        float innerRadius = light2D.pointLightInnerRadius;

 

        // Light2D�� ������ ��������, ���� Ȥ�� ���������� ���� ���
        Vector2 lightDirection = light2D.transform.up; // ���� �ٶ󺸴� ����
       
        // ���� �ݿ� ���·� ���� �׸���
        for (int i = 0; i < rayCount; i++)
        {
           
            // ������ ����Ͽ� ������ ������ ����
            float angle = Mathf.Lerp(-outerAngle / 2, outerAngle / 2, i / (float)(rayCount - 1));
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // ���� ���� ������ �������� �ݴ� ���⵵ ó��
            direction = Quaternion.FromToRotation(Vector2.right, lightDirection) * direction; // ���� ȸ��

            // ���̸� �ܺ� ������ ���̷� �߻�
            RaycastHit2D hit = Physics2D.Raycast(lightPosition, direction, outerRadius, enemyLayer);

            // ���� �׸��� (������)
            Debug.DrawRay(lightPosition, direction * outerRadius, Color.green);

            if (hit.collider != null)
            {
                if (hit.transform.GetComponent<Enemy>())
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    if (!checkEnemy.Contains(enemy)) // �ߺ��� ���ϱ� ���� üũ
                    {
                        checkEnemy.Add(enemy);
                        enemy.GetComponent<SpriteRenderer>().enabled = true;
                        is_E = true;
                        break;
                    }
                }
            }

            
         
        }

        yield return new WaitForSeconds(0.08f);

        if (is_E == false && checkEnemy.Count != 0)
        {
            for(int i=0;i< checkEnemy.Count; i++)
            {
                checkEnemy[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            checkEnemy.Clear();
        }
        StartCoroutine("find_Enemy");
    }




    //�� �� ����Ʈ ����
    public void Change_Light(bool isSun)
    {
      
        if (isSun)//���϶�
        {
            afternoon_light2D.gameObject.SetActive(true);
            night_light2D.gameObject.SetActive(false);
         

            light2D = afternoon_light2D;
         
        }
        else
        {
            night_light2D.gameObject.SetActive(true);
            afternoon_light2D.gameObject.SetActive(false);
            light2D = night_light2D;
        }


    }
}
