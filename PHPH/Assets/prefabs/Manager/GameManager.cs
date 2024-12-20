using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
public class GameManager : MonoBehaviour
{
    //����Ʈ
    public Light2D sun;
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

        // 6��~18��: �� / 18��~6��: ��
        if (curTime >= 6 && curTime <= 18)
        {
            // �� ����(6~18): ����(12��)���� ���� ����
            float normalizedTime = (curTime - 6) / 12f; // 6~18�ø� 0~1�� ����ȭ
            sun.intensity = Mathf.Lerp(0.02f, 1f, (1 - Mathf.Cos(normalizedTime * Mathf.PI)) / 2f); // Cosine���� � ��ȯ
        }
        else
        {
            // �� ����(18~6): ���� 1�ÿ��� ���� ���
            float nightTime = (curTime > 18) ? (curTime - 18) / 12f : (curTime + 6) / 12f; // 18~6�ø� 0~1�� ����ȭ
            sun.intensity = Mathf.Lerp(1f, 0.02f, (1 - Mathf.Cos(nightTime * Mathf.PI)) / 2f); // Cosine���� � ��ȯ
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

}
