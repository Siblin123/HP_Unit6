using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerControl : NetworkBehaviour
{
    public AudioListener Listener;

    float horizontalInput;
    float VerticalInput;

    Rigidbody2D rb;
    public Light2D player_light;
    public float moveSpeed = 5f;
    public Transform rayPos;
    public float rayDisance;
    public LayerMask wallLayer;

    public Vector3 movedir;
    private Vector2 rayDirection;

    public GameObject lastWall;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!IsOwner)
        {
            Listener.enabled = false;
            player_light.enabled = false;
        }   

    }

    void LateUpdate()
    {
       // Debug.Log("LateUpdate 실행 중");
    }

    void FixedUpdate()
    {
        // Debug.Log("FixedUpdate 실행 중"); 
        if (!IsOwner)
            return;

        HandleMovement();
    }
    void Update()
    {
        if (!IsOwner)
            return;

        Light_Raycast();
       // print("1");
    }

    private void HandleMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {

            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1f, 1f);
            rayDirection = horizontalInput < 0 ? Vector2.left : Vector2.right;

            if(horizontalInput<0)
            {
                transform.Translate(new Vector2(horizontalInput, -movedir.normalized.y) * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector2(horizontalInput, movedir.normalized.y) * moveSpeed * Time.deltaTime);
            }
           

        }

    }

    //빛을 건너편이 보이게  하는 레이케스트
    private void Light_Raycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayPos.position, rayDirection, rayDisance, wallLayer);

        if (hit.collider != null)
        {

            if (hit.collider.gameObject)
            {
                if (lastWall != null)
                    lastWall.GetComponent<ShadowCaster2D>().trimEdge = 0f;

                lastWall = hit.collider.gameObject;
                lastWall.GetComponent<ShadowCaster2D>().trimEdge = 1f;
            }
        }
        else
        {
            if (lastWall != null)
            {
                lastWall.GetComponent<ShadowCaster2D>().trimEdge = 0f;
                lastWall = null;    
            }
        }

        Debug.DrawRay(rayPos.position, rayDirection * rayDisance, Color.black);
       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Vector3 wallNormal = collision.contacts[0].normal;

            Vector3 moveDir = new Vector3(horizontalInput, 0, 0);

            movedir = Vector3.ProjectOnPlane(moveDir, wallNormal);
            movedir = new Vector3(Mathf.Abs(movedir.x), Mathf.Abs(movedir.y), Mathf.Abs(movedir.z));
            Debug.DrawRay(transform.position, movedir, Color.red, 2f); // 충돌 지점에서 법선 벡터를 빨간색으로 그립니다.
        }
    }

}
