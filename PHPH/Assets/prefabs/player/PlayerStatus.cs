using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System;
using Unity.Netcode.Components;
using static PlayerStatus;

public class PlayerStatus : PlayerGadget
{
    public Light2D player_light;
    Rigidbody2D rb;

    // Attack()
    public Vector3 mousePos;
    public Transform AttackPos;

    // status
    public float maxHp;
    public float hp;
    public float maxStamina;
    public float stamina;
    public float stamina_RegenSpeed;
    public float stamina_useSpeed;

    public int stamina_percent;

    float horizontalInput;
    float VerticalInput;

    // Move()
    public float moveSpeed = 5f;
    public float runSpeed = 5f;
    public float jumpPower = 5f;
    float curSpeed;

    public Vector3 movedir;
    Vector2 enterStairPos;

    public Vector2 rayDirection;

    // UI 관련
    [SerializeField] Slider hpBar;
    [SerializeField] Slider staminaBar;

    // 애니메이션
    public enum AnimationType
    {
        stand,
        walk,
        run,
        jump_up,
        jump_down,
        get_damage
    }

    public AnimationType AnimationState;

    public NetworkAnimator anim;

    public override void Start()
    {   

        if (!IsOwner)
            return;

        base.Start();
        init();
        rb = GetComponent<Rigidbody2D>();

      
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

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
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) && horizontalInput != 0 && stamina > 1)
        {
            if (rb.linearVelocityY == 0 && (AnimationState != AnimationType.jump_up || AnimationState != AnimationType.jump_down))
            {
                ChangeAnim(AnimationType.run);
            }
            curSpeed = runSpeed;
            stamina -= stamina_useSpeed * Time.deltaTime;
        }
        else
        {
            if (stamina <= maxStamina)
                stamina += stamina_RegenSpeed * Time.deltaTime;

            if (horizontalInput != 0)
            {
                if (rb.linearVelocityY == 0 &&  (AnimationState != AnimationType.jump_up || AnimationState != AnimationType.jump_down))
                {
                    ChangeAnim(AnimationType.walk);
                }
            }
            else if (rb.linearVelocityY==0&&(AnimationState != AnimationType.jump_up || AnimationState != AnimationType.jump_down))
            {
                print("aaa");
                ChangeAnim(AnimationType.stand);
                print("ccc");
            }
            curSpeed = moveSpeed;
        }

        transform.Translate(new Vector2(horizontalInput, 0) * curSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            ChangeAnim(AnimationType.jump_up);
        }

        if (rb.linearVelocity.y < -0.1f && AnimationState != AnimationType.jump_down)
        {
            print("cur1 : " + AnimationState);
            ChangeAnim(AnimationType.jump_down);
            print("cur2 : " + AnimationState);
        }

        if(rb.linearVelocity.y ==0 && AnimationState == AnimationType.jump_down)
        {
            print("cur3 : " + AnimationState);
            ChangeAnim(AnimationType.stand);
            print("cur4 : " + AnimationState);
        }
       
    }

    public void GetDamege(float value)
    {
        hp -= value;

        if (hp <= 0)
        {
            // 플레이어 사망 처리
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

        if (transform.position.x > mousePos.x)
        {
            transform.localScale = new Vector3(-1, 1f, 1f);
            AttackPos.transform.localScale = new Vector3(-1, -1f, 1f);
            player_light.transform.localScale = new Vector3(-1, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1, 1f, 1f);
            AttackPos.transform.localScale = new Vector3(1, 1f, 1f);
            player_light.transform.localScale = new Vector3(1, 1f, 1f);
        }
    }

    // 클라이언트에서 호출되는 메서드
    void ChangeAnim(AnimationType newanim)
    {
        if (IsClient)
        {
            // 서버에 애니메이션 변경 요청
            ChangeAnim_ServerRpc(newanim);
        }

        // 클라이언트에서 애니메이션 실행
        anim.Animator.Play(newanim.ToString());
        print(newanim.ToString() + "@@@@@@");

        // clip.name을 AnimationType으로 변환
        if (Enum.TryParse(typeof(AnimationType), newanim.ToString(), out var result))
        {
            AnimationState = (AnimationType)result;
            print(newanim.ToString() + "!!!!!!");
        }
    }

    // 서버에서 호출되는 메서드
    [ServerRpc(RequireOwnership = false)]
    public void ChangeAnim_ServerRpc(AnimationType newanim)
    {
        // 서버는 클라이언트에게 애니메이션 변경을 요청
        ChangeAnim_ClientRpc(newanim);
    }

    // 서버 -> 클라이언트로 애니메이션 변경 요청
    [ClientRpc]
    public void ChangeAnim_ClientRpc(AnimationType newanim)
    {
        // 클라이언트에서 애니메이션 실행
        anim.Animator.Play(newanim.ToString());
        print(newanim.ToString() + " (ClientRpc)!!!!!!");

        // clip.name을 AnimationType으로 변환
        if (Enum.TryParse(typeof(AnimationType), newanim.ToString(), out var result))
        {
            AnimationState = (AnimationType)result;
            print(newanim.ToString() + "!!!!!!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Vector3 wallNormal = collision.contacts[0].normal;

            Vector3 moveDir = new Vector3(horizontalInput, 0, 0);
            movedir = Vector3.ProjectOnPlane(moveDir, wallNormal).normalized;

            Debug.DrawRay(transform.position, movedir, Color.red, 2f);
        }

        if (collision.transform.CompareTag("Ground"))
        {
            movedir = Vector3.zero;
        }
    }
}
