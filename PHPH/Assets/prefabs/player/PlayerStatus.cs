using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class PlayerStatus : PlayerGadget
{
    public Light2D player_light;
    Rigidbody2D rb;

    //Attack()
    public Vector3 mousePos;
    public Transform AttackPos;
    //status
    public float maxHp;
    public float hp;
    
    public float maxStamina;
    public float stamina;
    public float stamina_RegenSpeed;
    public float stamina_useSpeed;

    float horizontalInput;
    float VerticalInput;
    //Move()
    public float moveSpeed = 5f;
    public float runSpeed = 5f;
    public float jumpPower = 5f;
    float curSpeed;
    public Vector3 movedir;
    Vector2 enterStairPos;//계단을 들어온 위치(왼,오)


    protected Vector2 rayDirection;

    public override void Start()
    {
        base.Start();
        init();
        rb = GetComponent<Rigidbody2D>();

    }



    //UI모음
    [SerializeField] Slider hpBar;
    [SerializeField] Slider staminaBar;
    public override void FixedUpdate()
    {
       
        if (!IsOwner)
            return;
       

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            csTable.Instance.gameManager.SendSound(transform.position);
        }
    }

    public override void Update()
    {
        if (!IsOwner)
            return;
        base.Update();
        Move();
        Jump();
        LookMouse();
    }

    public override void init()
    {
        base.init();
        hp = maxHp;
        stamina = maxStamina;

        hpBar.maxValue = maxHp;
        staminaBar.maxValue = maxStamina;

    }

    private void Move()
    {
        if (mousePos.x > transform.position.x)//벽 찾는 레이가 마우스 기준
            rayDirection = Vector2.right;
        else
            rayDirection = Vector2.left;



        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (horizontalInput != 0 && stamina >1)
            {
                curSpeed = runSpeed;
                stamina -= stamina_useSpeed * Time.deltaTime;
            }
               
        }
        else
        {
            if(stamina<=maxStamina)
                stamina += stamina_RegenSpeed * Time.deltaTime;

            curSpeed = moveSpeed;

        }



        if (horizontalInput != 0)
        {
            if(enterStairPos==Vector2.zero)
            {
                enterStairPos = rayDirection;
            }

            if (movedir == Vector3.zero)
            {
                transform.Translate(new Vector2(horizontalInput, 0) * curSpeed * Time.deltaTime);

            }
            else
            {
                transform.Translate(new Vector2(movedir.normalized.x, movedir.normalized.y) * curSpeed * Time.deltaTime);
                print("계단 이용중");
            }

            if(enterStairPos.x!= horizontalInput)
            {
                enterStairPos.x = horizontalInput;
                movedir *= -1;
            }

        }

        //UI
        staminaBar.value = stamina;
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocityY = 0;
            rb.linearVelocityY += jumpPower;
        }
    }

    public void GetDamege(float value)
    {
        hp -= value;

        if (hp <= 0)
        { //플레이어 사망
        
        }

        //UI
        hpBar.value = hp;

    }

    public void LookMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        AttackPos.transform.rotation = Quaternion.Euler(new Vector3(1, 0, angle));
        player_light.transform.rotation = Quaternion.Euler(new Vector3(1, 0, angle - 90));

        if (transform.position.x > mousePos.x) // 왼쪽을 바라볼 때
        {
            transform.localScale = new Vector3(-1, 1f, 1f);
            AttackPos.transform.localScale = new Vector3(-1, -1f, 1f);
            player_light.transform.localScale = new Vector3(-1, -1f, 1f);

        }
        else // 오른쪽을 바라볼 때
        {
            transform.localScale = new Vector3(1, 1f, 1f);
            AttackPos.transform.transform.localScale = new Vector3(1, 1f, 1f);
            player_light.transform.localScale = new Vector3(1, 1f, 1f);
        }

       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Vector3 wallNormal = collision.contacts[0].normal;

            Vector3 moveDir = new Vector3(horizontalInput, 0, 0);

            movedir = Vector3.ProjectOnPlane(moveDir, wallNormal);
            movedir = movedir.normalized;
            //movedir = new Vector3(Mathf.Abs(movedir.x), Mathf.Abs(movedir.y), Mathf.Abs(movedir.z));
            Debug.DrawRay(transform.position, movedir, Color.red, 2f); // 충돌 지점에서 법선 벡터를 빨간색으로 그립니다.
        }

        if(collision.transform.CompareTag("Ground"))
        {
            movedir = Vector3.zero;
        }
    }

}
