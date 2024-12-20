using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
public class GameManager : MonoBehaviour
{
    //라이트
    public Light2D sun;
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

        // 6시~18시: 낮 / 18시~6시: 밤
        if (curTime >= 6 && curTime <= 18)
        {
            // 낮 동안(6~18): 정오(12시)에서 가장 밝음
            float normalizedTime = (curTime - 6) / 12f; // 6~18시를 0~1로 정규화
            sun.intensity = Mathf.Lerp(0.02f, 1f, (1 - Mathf.Cos(normalizedTime * Mathf.PI)) / 2f); // Cosine으로 곡선 전환
        }
        else
        {
            // 밤 동안(18~6): 새벽 1시에서 가장 어둠
            float nightTime = (curTime > 18) ? (curTime - 18) / 12f : (curTime + 6) / 12f; // 18~6시를 0~1로 정규화
            sun.intensity = Mathf.Lerp(1f, 0.02f, (1 - Mathf.Cos(nightTime * Mathf.PI)) / 2f); // Cosine으로 곡선 전환
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

}
