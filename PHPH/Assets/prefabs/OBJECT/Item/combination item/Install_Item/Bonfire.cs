using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class Bonfire : Item_Info
{
    [SerializeField] Light2D Light;

    [Header("x~y�� ���̷� ���� ��Ⱑ �ٲ��")]
    public Vector2 light_magnitude_Size;
    [SerializeField] float speed;
    string go_X_Y = "X";

    public LayerMask enemyLayer; // ���� ���Ե� ���̾�
    public  List<Collider2D> previousEnemies;
    public override void Update()
    {
        base.Update();
        light_magnitude();
    }

    public void light_magnitude()
    {

        //
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, Light.pointLightOuterRadius, enemyLayer);

        // ���� Ž���� ������ ����Ʈ�� ��ȯ
        List<Collider2D> currentEnemies = new List<Collider2D>(hitEnemies);

        if(currentEnemies.Count==0 && previousEnemies.Count!=0)//���� �ƹ��� ������ ������ ���� ���� ��� �����ֱ�
        {
            foreach (Collider2D enemy in previousEnemies)
            {
                SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = false;
                }
            }
            previousEnemies = currentEnemies;

        }
        else
        {
            // ������ Ž���� ���� ��, �̹� �����ӿ� Ž������ ���� ���� �̹��� ����
            foreach (Collider2D enemy in previousEnemies)
            {
                if (!currentEnemies.Contains(enemy))
                {
                    SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.enabled = false;
                    }
                }
            }

            // ���� ���� �ȿ� �ִ� ������ �̹��� �ѱ�
            foreach (Collider2D enemy in currentEnemies)
            {
                SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = true;
                }
            }
            previousEnemies = currentEnemies;
        }
       

        //=============������������Ƚ�ҿ� ���� �� �� ��� ���̱������������������


        // �̹� �������� �� ����Ʈ�� ���� �� ����Ʈ�� �����Ͽ� ���� �����ӿ� ���


        if (go_X_Y=="X")
        {
            Light.intensity += Time.deltaTime * speed;
        }
        else
        {
            Light.intensity -= Time.deltaTime * speed;
        }

        if (Light.intensity < light_magnitude_Size.x)
        {
            //Ŀ������
            go_X_Y="X";
        }
        else if(Light.intensity > light_magnitude_Size.y)
        {
            //�۾�������
            go_X_Y="Y";
        }
   
    }

    private void OnDrawGizmos()
    {
        if (Light == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Light.pointLightOuterRadius);
    }
}
