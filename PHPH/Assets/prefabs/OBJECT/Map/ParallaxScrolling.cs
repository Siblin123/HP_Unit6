using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    private Material mat;
    [SerializeField] private float distance;

    [Range(0, 0.2f)]
    public float speed = 0.5f; // ��� ��ũ�� �ӵ� ����
    private float previousInput = 0; // ���� �Է� ���� �����Ͽ� ������ ����

    public bool autoMove;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {


        // ĳ���Ͱ� �ִ��� Ȯ��
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

                // ĳ������ ���� �Է� �� ��������
                float horizontalInput = player.horizontalInput;

                // ���� �ӵ��� �Է°����� �̵� �Ÿ� ���
                distance += Time.deltaTime * speed * player.curSpeed * Mathf.Lerp(previousInput, horizontalInput, 0.1f) * 0.1f;


                // �ؽ�ó ������ ������Ʈ
                mat.SetTextureOffset("_MainTex", new Vector2(distance, 0));

                // ���� �Է� �� ����
                previousInput = horizontalInput;
            }

          
        }
    }
}
