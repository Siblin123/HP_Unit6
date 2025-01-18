using UnityEngine;

public class Parallax_Auto : MonoBehaviour
{
    public float speed;  // �⺻ �̵� �ӵ�
    public Transform start_pos;  // ó�� ��ġ
    public Transform last_pos;   // ������ ��ġ
    private PlayerControl player;  // �÷��̾�

    private float targetXPosition;  // Ÿ�� X ��ǥ (�ε巯�� �̵��� ����)

    public bool isSky;
    void Start()
    {
        if (csTable.Instance.gameManager.player != null)
            player = csTable.Instance.gameManager.player;
    }

    private void FixedUpdate()
    {
        if (isSky)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, transform.localPosition + Vector3.right, 0.1f);  // 0.1f�� �̵� �ӵ� ����

            // last_pos�� �Ѿ�� �ʵ��� ����
            if (transform.localPosition.x > last_pos.position.x)
            {
                transform.localPosition = new Vector3(start_pos.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.position.x < start_pos.position.x)
            {
                transform.localPosition = new Vector3(last_pos.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            }

            return;
        }


        if (player == null)
        {
            // �÷��̾ �����ϴ� ��� player ��ü�� �����ɴϴ�.
            if (csTable.Instance.gameManager.player != null)
                player = csTable.Instance.gameManager.player;
        }

        // �÷��̾��� �Է°� �ӵ��� ���� �̵�
        float moveAmount = player.horizontalInput * speed * player.curSpeed * Time.deltaTime;

        // �ε巯�� ��� �̵� (Lerp ���)
        targetXPosition = transform.position.x + moveAmount;
        float smoothX = Mathf.Lerp(transform.position.x, targetXPosition, 0.1f);  // 0.1f�� �̵� �ӵ� ����
        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);

        // last_pos�� �Ѿ�� �ʵ��� ����
        if (transform.position.x > last_pos.position.x)
        {
            transform.position = new Vector3(start_pos.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < start_pos.position.x)
        {
            transform.position = new Vector3(last_pos.position.x, transform.position.y, transform.position.z);
        }
    }
}
