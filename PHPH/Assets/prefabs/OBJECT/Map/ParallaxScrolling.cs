using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    private Material mat;
    [SerializeField] private float distance;

    [Range(0, 0.2f)]
    public float speed = 0.5f; // 배경 스크롤 속도 조정
    private float previousInput = 0; // 이전 입력 값을 저장하여 끊김을 줄임

    public bool autoMove;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {


        // 캐릭터가 있는지 확인
        if (csTable.Instance?.gameManager?.player != null)
        {
            if (autoMove)
            {
                distance += Time.deltaTime * speed;
                mat.SetTextureOffset("_MainTex", new Vector2(distance, 0));
            }
            else
            {
                var player = csTable.Instance.gameManager.player;

                // 캐릭터의 수평 입력 값 가져오기
                float horizontalInput = player.horizontalInput;

                // 현재 속도와 입력값으로 이동 거리 계산
                distance += Time.deltaTime * speed * player.curSpeed * Mathf.Lerp(previousInput, horizontalInput, 0.1f) * 0.1f;


                // 텍스처 오프셋 업데이트
                mat.SetTextureOffset("_MainTex", new Vector2(distance, 0));

                // 이전 입력 값 갱신
                previousInput = horizontalInput;
            }

          
        }
    }
}
