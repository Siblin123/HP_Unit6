using UnityEngine;

public class GoSoundMonster : Enemy
{
    public float hearingThreshold = 0.5f; // 소리 감지 민감도 (0~1 사이)
    public float reactionTime = 1f; // 소리를 감지하고 반응하기까지 시간

    private Vector3 lastHeardPosition; // 마지막으로 소리가 들린 위치
    private bool isInvestigating = false;

    public void OnHearNoise(Vector3 noisePosition)
    {
        float distance = Vector3.Distance(noisePosition, transform.position);

       
    
        if (distance < hearingThreshold)
        {
            lastHeardPosition = noisePosition;
            print("범위내에 플레이어가 있음");
        }
        else
        {
            print("distance : "+ distance +"@@" + "hearingThreshold : " + hearingThreshold);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // 몬스터 위치를 기준으로 원 그리기
        Gizmos.color = new Color(1, 0, 0, 0.3f); // 반투명 빨간색
        Gizmos.DrawSphere(transform.position, hearingThreshold); // 소리 감지 반경

        // 감지 거리의 외곽선
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingThreshold);
    }

}
