using UnityEngine;

public class Parallax_Auto : MonoBehaviour
{
    public float speed;  // 기본 이동 속도
    public Transform start_pos;  // 처음 위치
    public Transform last_pos;   // 마지막 위치
    private PlayerControl player;  // 플레이어

    private float targetXPosition;  // 타겟 X 좌표 (부드러운 이동을 위한)

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
            transform.localPosition = Vector2.Lerp(transform.localPosition, transform.localPosition + Vector3.right, 0.1f);  // 0.1f는 이동 속도 조절

            // last_pos를 넘어가지 않도록 제한
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
            // 플레이어가 존재하는 경우 player 객체를 가져옵니다.
            if (csTable.Instance.gameManager.player != null)
                player = csTable.Instance.gameManager.player;
        }

        // 플레이어의 입력과 속도에 따라 이동
        float moveAmount = player.horizontalInput * speed * player.curSpeed * Time.deltaTime;

        // 부드러운 배경 이동 (Lerp 사용)
        targetXPosition = transform.position.x + moveAmount;
        float smoothX = Mathf.Lerp(transform.position.x, targetXPosition, 0.1f);  // 0.1f는 이동 속도 조절
        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);

        // last_pos를 넘어가지 않도록 제한
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
