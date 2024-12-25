using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
public class GameManager : MonoBehaviour
{
    //����Ʈ
    public Light2D sun;

    public int is_afterNoonNight;

    public float curTime;
    public float daySpeed;//�Ϸ� �ð��� �ӵ�

    public int survivalDay;

    public List<Enemy> all_SpawnMonster;


    private void Update()
    {
        Day();

    }

    public void Day()
    {

       

        // �ð� ����
        curTime += Time.deltaTime * daySpeed;

        if (curTime > 18 && curTime <= 19)
        {
            if (is_afterNoonNight == 0)
            {
                is_afterNoonNight = 1; 
                All_Stair_trimEdge_RESET(is_afterNoonNight);
            }
                
            // 18��~19��: ���� ��ο���
            float transitionTime = (curTime - 18); // 18~19�ø� 0~1�� ����ȭ
            sun.intensity = Mathf.Lerp(1f, 0.02f, transitionTime);
        }
        else if (curTime >= 5 && curTime < 6)
        {
            if (is_afterNoonNight == 1)
            {
                is_afterNoonNight = 0;
                All_Stair_trimEdge_RESET(is_afterNoonNight);
            }
                
            // 5��~6��: ���� �����
            float transitionTime = (curTime - 5); // 5~6�ø� 0~1�� ����ȭ
            sun.intensity = Mathf.Lerp(0.02f, 1f, transitionTime);
        }

        // �Ϸ簡 ������ �ʱ�ȭ
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

    void All_Stair_trimEdge_RESET(int isAfterNoon_Night)//�㳷�� �ٲ� ����� trimEdge�� �ʱ�ȭ
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
