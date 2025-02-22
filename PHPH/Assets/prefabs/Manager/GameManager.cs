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
    public bool isStartGame = false;


    //라이트
    public Light2D sun;

    public NetworkVariable<int> is_afterNoonNight = new NetworkVariable<int>(0);
    public float curTime;
    public float daySpeed;//하루 시간의 속도


    public NetworkVariable<int> survivalDay = new NetworkVariable<int>(0);
    public List<Enemy> all_SpawnMonster;

    [Header("작물 및 오브젝트 생성을 위한 변수")]
    public GameObject Map_Grid;
    public List<Map_ObjectCreateManager> map_ObjectCreateManagers;

    private void Start()
    {
       
        //Map_Grid의 자식중에 Map_ObjectCreateManager를 찾아서 리스트에 추가
        map_ObjectCreateManagers.AddRange(Map_Grid.GetComponentsInChildren<Map_ObjectCreateManager>());
    }

    private void Update()
    {
        if (isStartGame)
        {
            Day();

        }

    }

    public void Day()
    {
        if (IsServer)
            curTime += Time.deltaTime * daySpeed;

        if (curTime > 18 && curTime <= 19)
        {
            if (is_afterNoonNight.Value == 0)
            {
                is_afterNoonNight.Value = 1;
                ChageSun_ClientRpc();

            }


        }
        else if (curTime >= 5 && curTime < 6)
        {
            if (is_afterNoonNight.Value == 1)
            {
                is_afterNoonNight.Value = 0;
                ChageSun_ClientRpc();
            }


        }

        // 하루가 끝나면 초기화
        if (curTime >= 24)
        {
            CheckGameRes();

            survivalDay.Value++;
            curTime = 0;

            // 모든 개체에 대해 Create_mapObject() 실행
            foreach (var manager in map_ObjectCreateManagers)
            {
                manager.Create_mapObject();
            }
        }
    }

    [ClientRpc]
    public void ChageSun_ClientRpc()
    {
        StartCoroutine(ChageSun());
    }
    IEnumerator ChageSun()
    {
        float transitionTime = 0;
        while (true)
        {
            transitionTime += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
            if (is_afterNoonNight.Value == 0)
            {  // 5시~6시: 점점 밝아짐
                sun.intensity = Mathf.Lerp(0.02f, 1f, transitionTime);
            }
            else
            {
                // 18시~19시: 점점 어두워짐

                sun.intensity = Mathf.Lerp(1f, 0.02f, transitionTime);
            }

            if (transitionTime >= 1)
            {
                break;
            }
        }


    }


    public void CheckLight()
    {
        if (is_afterNoonNight.Value == 0)
        {  // 5시~6시: 점점 밝아짐
            sun.intensity =1;
        }
        else
        {
            // 18시~19시: 점점 어두워짐

            sun.intensity = 0.02f;
        }
    }

    void CheckGameRes()
    {
        //현재 가지고 있는 돈보다 작으면 게임오버

    }


    //=============================================아래 부턴 매니저를 필요로 하는 오브젝트들의 함수=============================================

    public void SendSound(Vector3 soundPos)//사운드를 듣는 모든 몬스터에게 사운드를 전달
    {
        foreach (Enemy enemy in all_SpawnMonster)
        {
            if(enemy.GetComponent<GoSoundMonster>())
            {
                enemy.GetComponent<GoSoundMonster>().OnHearNoise(soundPos);
            }
        }
    }

   

}
