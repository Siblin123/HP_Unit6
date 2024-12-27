using UnityEngine;

public class GoSoundMonster : Enemy
{
    public float hearingThreshold = 0.5f; // �Ҹ� ���� �ΰ��� (0~1 ����)
    public float reactionTime = 1f; // �Ҹ��� �����ϰ� �����ϱ���� �ð�

    private Vector3 lastHeardPosition; // ���������� �Ҹ��� �鸰 ��ġ
    private bool isInvestigating = false;

    public void OnHearNoise(Vector3 noisePosition)
    {
        float distance = Vector3.Distance(noisePosition, transform.position);

       
    
        if (distance < hearingThreshold)
        {
            lastHeardPosition = noisePosition;
            print("�������� �÷��̾ ����");
        }
        else
        {
            print("distance : "+ distance +"@@" + "hearingThreshold : " + hearingThreshold);
        }
    }
    private void OnDrawGizmosSelected()
    {
        // ���� ��ġ�� �������� �� �׸���
        Gizmos.color = new Color(1, 0, 0, 0.3f); // ������ ������
        Gizmos.DrawSphere(transform.position, hearingThreshold); // �Ҹ� ���� �ݰ�

        // ���� �Ÿ��� �ܰ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingThreshold);
    }

}
