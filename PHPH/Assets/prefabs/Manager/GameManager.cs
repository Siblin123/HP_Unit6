using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
public class GameManager : MonoBehaviour
{
    //라이트
    public Light2D sun;

    public int is_afterNoonNight;

    public float curTime;
    public float daySpeed;//하루 시간의 속도

    public int survivalDay;

    public List<Enemy> all_SpawnMonster;


    private void Update()
    {
        Day();

    }

    public void Day()
    {

       

        // 시간 증가
        curTime += Time.deltaTime * daySpeed;

        if (curTime > 18 && curTime <= 19)
        {
            if (is_afterNoonNight == 0)
            {
                is_afterNoonNight = 1; 
                All_Stair_trimEdge_RESET(is_afterNoonNight);
            }
                
            // 18시~19시: 점점 어두워짐
            float transitionTime = (curTime - 18); // 18~19시를 0~1로 정규화
            sun.intensity = Mathf.Lerp(1f, 0.02f, transitionTime);
        }
        else if (curTime >= 5 && curTime < 6)
        {
            if (is_afterNoonNight == 1)
            {
                is_afterNoonNight = 0;
                All_Stair_trimEdge_RESET(is_afterNoonNight);
            }
                
            // 5시~6시: 점점 밝아짐
            float transitionTime = (curTime - 5); // 5~6시를 0~1로 정규화
            sun.intensity = Mathf.Lerp(0.02f, 1f, transitionTime);
        }

        // 하루가 끝나면 초기화
        if (curTime >= 24)
        {
            survivalDay++;
            curTime = 0;
        }
    }



    public void SendSound(Vector3 soundPos)
    {
        foreach (Enemy enemy in all_SpawnMonster)
        {
            if(enemy.GetComponent<GoSoundMonster>())
            {
                enemy.GetComponent<GoSoundMonster>().OnHearNoise(soundPos);
            }
        }
    }

    void All_Stair_trimEdge_RESET(int isAfterNoon_Night)//밤낮이 바뀔때 계단의 trimEdge를 초기화
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in walls)
        {
            ShadowCaster2D shadowCaster = wall.GetComponent<ShadowCaster2D>();
            if (shadowCaster != null)
            {

                if(isAfterNoon_Night==0)
                {
                    shadowCaster.trimEdge = 1f;
                }
                else if (isAfterNoon_Night == 1)
                {
                    shadowCaster.trimEdge = 0f;
                }


               
            }
        }
    }

}
