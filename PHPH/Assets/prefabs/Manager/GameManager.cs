using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.Netcode;
using System.Collections;
using static System.TimeZoneInfo;
public class GameManager : NetworkBehaviour
{
    public PlayerControl player;
    public bool isStartGame=false;


    //����Ʈ
    public Light2D sun;

    public NetworkVariable<int> is_afterNoonNight = new NetworkVariable<int>(0);
    public float curTime;
    public float daySpeed;//�Ϸ� �ð��� �ӵ�


    public  NetworkVariable<int> survivalDay = new NetworkVariable<int>(0);
    public List<Enemy> all_SpawnMonster;


    private void Update()
    {
        if(isStartGame)
        {
            Day();

        }

    }

    public void Day()
    {
        if(IsServer)
            curTime += Time.deltaTime * daySpeed;

        if (curTime > 18 && curTime <= 19)
        {
            if (is_afterNoonNight.Value == 0)
            {
                is_afterNoonNight.Value = 1; 
                All_Stair_trimEdge_RESET(is_afterNoonNight.Value);
                ChageSun_ClientRpc();

            }
               

        }
        else if (curTime >= 5 && curTime < 6)
        {
            if (is_afterNoonNight.Value == 1)
            {
                is_afterNoonNight.Value = 0;
                All_Stair_trimEdge_RESET(is_afterNoonNight.Value);
                ChageSun_ClientRpc();
            }
                
           
        }

        // �Ϸ簡 ������ �ʱ�ȭ
        if (curTime >= 24)
        {
            survivalDay.Value++;
            curTime = 0;
        }
    }

    [ClientRpc]
    void ChageSun_ClientRpc()
    {
        StartCoroutine(ChageSun());
    }
    IEnumerator ChageSun()
    {

        float transitionTime = 0;
        while (true)
        {
            transitionTime+=Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
            if (is_afterNoonNight.Value == 0)
            {  // 5��~6��: ���� �����
                sun.intensity = Mathf.Lerp(0.02f, 1f, transitionTime);
            }
            else
            {
                // 18��~19��: ���� ��ο���

                sun.intensity = Mathf.Lerp(1f, 0.02f, transitionTime);
            }

            if(transitionTime>=1)
            {
                break;
            }
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
