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

 

        // Light2D의 방향을 기준으로, 왼쪽 혹은 오른쪽으로 레이 쏘기
        Vector2 lightDirection = light2D.transform.up; // 빛이 바라보는 방향
       
        // 빛의 반원 형태로 레이 그리기
        for (int i = 0; i < rayCount; i++)
        {
           
            // 각도를 계산하여 레이의 방향을 결정
            float angle = Mathf.Lerp(-outerAngle / 2, outerAngle / 2, i / (float)(rayCount - 1));
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // 현재 빛의 방향을 기준으로 반대 방향도 처리
            direction = Quaternion.FromToRotation(Vector2.right, lightDirection) * direction; // 방향 회전

            // 레이를 외부 반지름 길이로 발사
            RaycastHit2D hit = Physics2D.Raycast(lightPosition, direction, outerRadius, enemyLayer);

            // 레이 그리기 (디버깅용)
            Debug.DrawRay(lightPosition, direction * outerRadius, Color.green);

            if (hit.collider != null)
            {
                if (hit.transform.GetComponent<Enemy>())
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    if (!checkEnemy.Contains(enemy)) // 중복을 피하기 위해 체크
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
