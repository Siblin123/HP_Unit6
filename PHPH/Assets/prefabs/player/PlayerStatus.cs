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

    public int stamina_percent;

    float horizontalInput;
    float VerticalInput;
    //Move()
    public float moveSpeed = 5f;
    public float runSpeed = 5f;
    public float jumpPower = 5f;
    float curSpeed;
    public Vector3 movedir;
    Vector2 enterStairPos;//����� ���� ��ġ(��,��)


    public Vector2 rayDirection;

    //UI����
    [SerializeField] Slider hpBar;
    [SerializeField] Slider staminaBar;


    //�ִϸ��̼�
    public enum AnimationType
    {
        IDLE,
        WALK,
        RUN,
        JUMP,
        JUMPDown,
        GETDAMEGE
    }
    public AnimationType animationType;

    public Animator anim;
    public AnimationClip[] animationClips;//0 �⺻ 1 �ȱ� 2 �ٱ� 3 ���� 4�ǰ�

    public override void Start()
    {
        if (!IsOwner)
            return;
        base.Start();
        init();
        rb = GetComponent<Rigidbody2D>();

    }



    public override void FixedUpdate()
    {
       
        if (!IsOwner)
            return;

        Move();
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
       
        Jump();
        LookMouse();
        UI_View();
        Change_Ani();   

    }

    public override void init()
    {
        base.init();
        hp = maxHp;
        stamina = maxStamina;

        hpBar.maxValue = maxHp;
        staminaBar.maxValue = maxStamina;

    }

    void UI_View()
    {
        hpBar.value = hp;
        staminaBar.value = stamina;
    }

    private void Move()
    {
        if (mousePos.x > transform.position.x)//�� ã�� ���̰� ���콺 ����
            rayDirection = Vector2.right;
        else
            rayDirection = Vector2.left;



        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (horizontalInput != 0 && stamina >1)
            {
                if (animationType != AnimationType.JUMP || animationType != AnimationType.JUMPDown)
                {
                    animationType = AnimationType.RUN;
                }
                curSpeed = runSpeed;
                stamina -= stamina_useSpeed * Time.deltaTime;
            }
               
        }
        else
        {
            if(stamina<=maxStamina)
                stamina += stamina_RegenSpeed * Time.deltaTime;
            if (animationType != AnimationType.JUMP || animationType != AnimationType.JUMPDown)
            {
                animationType = AnimationType.WALK;
            }
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
                print("��� �̿���");
            }

            if(enterStairPos.x!= horizontalInput)
            {
                enterStairPos.x = horizontalInput;
                movedir *= -1;
            }

        }
        else
        {
            if(animationType!=AnimationType.JUMP || animationType != AnimationType.JUMPDown)
            {
                animationType = AnimationType.IDLE;
            }
          
        }

      
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocityY = 0;
            rb.linearVelocityY += jumpPower;
            animationType = AnimationType.JUMP;
        }

            
        if (rb.linearVelocityY <= -0.1f)
        {
            animationType = AnimationType.JUMPDown;
        }
    }

    public void GetDamege(float value)
    {
        animationType = AnimationType.GETDAMEGE;
        hp -= value;

        if (hp <= 0)
        { //�÷��̾� ���
        
        }

      

    }

    public void LookMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        AttackPos.transform.rotation = Quaternion.Euler(new Vector3(1, 0, angle));
        player_light.transform.rotation = Quaternion.Euler(new Vector3(1, 0, angle - 90));

        if (transform.position.x > mousePos.x) // ������ �ٶ� ��
        {
            transform.localScale = new Vector3(-1, 1f, 1f);
            AttackPos.transform.localScale = new Vector3(-1, -1f, 1f);
            player_light.transform.localScale = new Vector3(-1, -1f, 1f);

        }
        else // �������� �ٶ� ��
        {
            transform.localScale = new Vector3(1, 1f, 1f);
            AttackPos.transform.transform.localScale = new Vector3(1, 1f, 1f);
            player_light.transform.localScale = new Vector3(1, 1f, 1f);
        }

       
    }

    public void Change_Ani()
    {
        switch(animationType)
        {
            case AnimationType.IDLE:
                anim.Play(animationClips[0].name);
                break;
            case AnimationType.WALK:
                anim.Play(animationClips[1].name);
                break;
            case AnimationType.RUN:
                anim.Play(animationClips[2].name);
                break;
            case AnimationType.JUMP:
                anim.Play(animationClips[3].name);
                break;
            case AnimationType.GETDAMEGE:
                anim.Play(animationClips[4].name);
                break;
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
            Debug.DrawRay(transform.position, movedir, Color.red, 2f); // �浹 �������� ���� ���͸� ���������� �׸��ϴ�.
        }

        if(collision.transform.CompareTag("Ground"))
        {
            movedir = Vector3.zero;
        }
    }

}
