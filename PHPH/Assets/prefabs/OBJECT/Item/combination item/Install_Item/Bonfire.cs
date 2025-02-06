using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class Bonfire : Item_Info
{
    [SerializeField] Light2D Light;

    [Header("x~y축 사이로 빛의 밝기가 바뀐다")]
    public Vector2 light_magnitude_Size;
    [SerializeField] float speed;
    string go_X_Y = "X";

    public LayerMask enemyLayer; // 적이 포함된 레이어
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

        // 현재 탐지된 적들을 리스트로 변환
        List<Collider2D> currentEnemies = new List<Collider2D>(hitEnemies);

        if(currentEnemies.Count==0 && previousEnemies.Count!=0)//적이 아무도 없으면 이전의 적들 전부 모습 숨겨주기
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
            // 이전에 탐지된 적들 중, 이번 프레임에 탐지되지 않은 적의 이미지 끄기
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

            // 현재 범위 안에 있는 적들의 이미지 켜기
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
       

        //=============↑↑↑↑↑↑↑↑↑↑↑횟불에 있을 때 적 모습 보이기↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑


        // 이번 프레임의 적 리스트를 이전 적 리스트로 저장하여 다음 프레임에 사용


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
            //커저야함
            go_X_Y="X";
        }
        else if(Light.intensity > light_magnitude_Size.y)
        {
            //작아저야함
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
