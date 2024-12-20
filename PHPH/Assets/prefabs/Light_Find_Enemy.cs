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
    public List<Enemy> lastCheckEnemy; // �߰ߵ� ������ ������ ����Ʈ
    public bool isE;


    private void Start()
    {
        if (!IsOwner)
            return;
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

        FindEnemy();
    }

    void FindEnemy()
    {

        Vector2 lightPosition = light2D.transform.position;
        float outerRadius = light2D.pointLightOuterRadius;
        float outerAngle = light2D.pointLightOuterAngle; // ����Ʈ�� �ܺ� ����
        Vector2 lightDirection = light2D.transform.up;  // ���� ���� (Y�� ����)
        lastCheckEnemy.Clear();

        int i = 0;
        for (; i < rayCount; i++)
        {
            // Light2D�� Outer Angle�� ������� ���� ���� ��� (����� ����)
            float angle = Mathf.Lerp(-outerAngle / 2, outerAngle / 2, i / (float)(rayCount - 1)); // -OuterAngle/2 ~ OuterAngle/2
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // ���� ���� ����(lightDirection)�� �������� ���� ���� ȸ��
            direction = Quaternion.FromToRotation(Vector2.right, lightDirection) * direction;

            // Outer ������ �Ÿ����� ���� �߻�
            RaycastHit2D hit = Physics2D.Raycast(lightPosition, direction, outerRadius, enemyLayer);

            // ����׿� ���� �ð�ȭ
            Debug.DrawRay(lightPosition, direction * outerRadius, Color.green);

            // �� ����
            if (hit.collider != null)
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
             
                if (enemy != null && !lastCheckEnemy.Contains(enemy))
                {
                    lastCheckEnemy.Add(enemy);

                }
                if (enemy != null && !checkEnemy.Contains(enemy))
                {
                    checkEnemy.Add(enemy); // ���� ������ �� �߰�
                    
                    enemy.GetComponent<SpriteRenderer>().enabled = true; // ���� �� Ȱ��ȭ
                    print("���� ������");
                    isE = true;

                }
            }

        }

        if(checkEnemy.Count != lastCheckEnemy.Count)
        {
            foreach (Enemy e in checkEnemy)
            {
                e.GetComponent<SpriteRenderer>().enabled = false; // ���� �� Ȱ��ȭ
            }

            checkEnemy.Clear();
            checkEnemy = new List<Enemy>(lastCheckEnemy);

        }
        
       
        foreach (Enemy e in checkEnemy)
        {
            e.GetComponent<SpriteRenderer>().enabled = true; // ���� �� Ȱ��ȭ
        }
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
