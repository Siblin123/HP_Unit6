using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    public Light2D lightSource; // 빛 소스를 연결
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsInLightRange())
        {
            spriteRenderer.enabled = true; // 빛 범위 안에 있으면 보이게
        }
        else
        {
            spriteRenderer.enabled = false; // 빛 범위 밖에 있으면 가리기
        }
    }

    // 오브젝트가 빛 범위 안에 있는지 체크
    bool IsInLightRange()
    {
        if (lightSource == null)
            return false;

        // Light2D의 범위와 오브젝트의 위치 비교
        float distance = Vector2.Distance(transform.position, lightSource.transform.position);
        return distance <= lightSource.pointLightOuterRadius; // 빛의 외부 반경 범위 내에 있는지 체크
    }
}
