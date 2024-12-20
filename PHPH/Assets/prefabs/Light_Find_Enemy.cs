using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;

public class Light_Find_Enemy : NetworkBehaviour
{
    public Light2D afternoon_light2D; // Light2D 컴포넌트
    public Light2D night_light2D; // Light2D 컴포넌트
    public Light2D light2D; // Light2D 컴포넌트
    public LayerMask enemyLayer; // 적이 포함된 레이어
    public float rayLength = 10f; // 레이의 길이
    public int rayCount = 36; // 반원 모양의 레이 갯수
    public float outerAngle = 180f; // 빛의 외부 각도

    public List<Enemy> checkEnemy; // 발견된 적들을 저장할 리스트
    public List<Enemy> lastCheckEnemy; // 발견된 적들을 저장할 리스트
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
        float outerAngle = light2D.pointLightOuterAngle; // 라이트의 외부 각도
        Vector2 lightDirection = light2D.transform.up;  // 빛의 방향 (Y축 기준)
        lastCheckEnemy.Clear();

        int i = 0;
        for (; i < rayCount; i++)
        {
            // Light2D의 Outer Angle을 기반으로 레이 방향 계산 (노란색 영역)
            float angle = Mathf.Lerp(-outerAngle / 2, outerAngle / 2, i / (float)(rayCount - 1)); // -OuterAngle/2 ~ OuterAngle/2
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // 빛의 현재 방향(lightDirection)을 기준으로 레이 방향 회전
            direction = Quaternion.FromToRotation(Vector2.right, lightDirection) * direction;

            // Outer 반지름 거리까지 레이 발사
            RaycastHit2D hit = Physics2D.Raycast(lightPosition, direction, outerRadius, enemyLayer);

            // 디버그용 레이 시각화
            Debug.DrawRay(lightPosition, direction * outerRadius, Color.green);

            // 적 감지
            if (hit.collider != null)
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
             
                if (enemy != null && !lastCheckEnemy.Contains(enemy))
                {
                    lastCheckEnemy.Add(enemy);

                }
                if (enemy != null && !checkEnemy.Contains(enemy))
                {
                    checkEnemy.Add(enemy); // 새로 감지된 적 추가
                    
                    enemy.GetComponent<SpriteRenderer>().enabled = true; // 현재 적 활성화
                    print("적을 감지함");
                    isE = true;

                }
            }

        }

        if(checkEnemy.Count != lastCheckEnemy.Count)
        {
            foreach (Enemy e in checkEnemy)
            {
                e.GetComponent<SpriteRenderer>().enabled = false; // 현재 적 활성화
            }

            checkEnemy.Clear();
            checkEnemy = new List<Enemy>(lastCheckEnemy);

        }
        
       
        foreach (Enemy e in checkEnemy)
        {
            e.GetComponent<SpriteRenderer>().enabled = true; // 현재 적 활성화
        }
    }


    //밤 낮 라이트 변경
    public void Change_Light(bool isSun)
    {
      
        if (isSun)//낮일때
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
