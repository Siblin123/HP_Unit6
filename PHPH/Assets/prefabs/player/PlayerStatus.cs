using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System;
using Unity.Netcode.Components;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;
using Unity.Services.Vivox;
using static Item_Info;

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

    [HideInInspector]public float horizontalInput;
    float VerticalInput;

    // Move()
    public float moveSpeed = 5f;
    public float runSpeed = 5f;
    public float jumpPower = 5f;
    public float curSpeed;

    public Vector3 movedir;
    Vector2 enterStairPos;

    public Vector2 rayDirection;

    // UI 관련
    [SerializeField] Slider hpBar;
    [SerializeField] Slider staminaBar;

    // 애니메이션
    public enum AnimationType
    {
        //이동
        stand,
        walk,
        run,
        jump_up,
        jump_down,
        get_damage
    }


    public AnimationType AnimationState;

    public NetworkAnimator anim;

    //getItem()
    RaycastHit2D hit_Item;
    [SerializeField] Item_Info itemmmmm;

    //getDamage()
    [Header("무적")]
    public float invincibility_Time;//무적 시간
    [SerializeField] float invincibility_Time_Cur;

    public override void Start()
    {   

        if (!IsOwner)
            return;

        base.Start();
        init();
        rb = GetComponent<Rigidbody2D>();
        csTable.Instance.gameManager.isStartGame = true;

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
/*        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            csTable.Instance.gameManager.SendSound(transform.position);
        }*/
    }

    public override void Update()
    {
        if (!IsOwner)
            return;

        base.Update();

        Jump();
        LookMouse();
        UI_View();
        GetItem();


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
        if (IsOwner) 
        {
            hpBar.value = hp;
            staminaBar.value = stamina;
        }

    }

    private void Move()
    {


        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (AnimationState == AnimationType.get_damage)
            return;


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
                    ChangeAnim(AnimationType.walk   );
                }
            }
            else if (rb.linearVelocityY==0&&(AnimationState != AnimationType.jump_up || AnimationState != AnimationType.jump_down))
            {
                ChangeAnim(AnimationType.stand);
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
            ChangeAnim(AnimationType.jump_down);
        }

        if(rb.linearVelocity.y ==0 && AnimationState == AnimationType.jump_down)
        {
            ChangeAnim(AnimationType.stand);
        }
       
    }
    //===================================================
    public void GetItem()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Vector2 boxSize = GetComponent<Collider2D>().bounds.size;
            hit_Item = Physics2D.BoxCast(transform.position + new Vector3(0, 0.16f, 0), boxSize, 0f, Vector2.zero, 0f, LayerMask.GetMask("Item"));

            if(hit_Item)
            {
                Item_Info info = hit_Item.transform.GetComponent<Item_Info>();
                itemmmmm = info;
                if (info.curItemType == itemType.combination_Item_Installable)
                    return;


                transform.GetComponent<Player_Inventory>().Get_Item(info, info.have_Count);

            

                if (IsServer)
                {
                    GetItem_ClientRpc(itemmmmm.GetComponent<NetworkObject>().NetworkObjectId);
                }
                else
                {
                    GetItem_ServerRpc(itemmmmm.GetComponent<NetworkObject>().NetworkObjectId);
                }
            }



        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetItem_ServerRpc(ulong id)
    {
        
        GetItem_ClientRpc(id);

    }

    [ClientRpc]
    public void GetItem_ClientRpc(ulong id)
    {


        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out NetworkObject networkObject))
        {

            networkObject.transform.gameObject.SetActive(false);

        }


    }


    //==========================================

    public void Get_damage(float value , Transform getPos )
    {
        if (!IsOwner)
            return;

        //무적일 경우
        if(AnimationState == AnimationType.get_damage)
            return;

        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[NetworkObjectId];
        if (networkObject != null && networkObject.TryGetComponent<Rigidbody2D>(out var rb))
        {
            Vector3 KnockbackDir = (transform.position - getPos.position).normalized;
                
            rb.AddForce(new Vector2(KnockbackDir.x ,1) , ForceMode2D.Impulse);
        }

        //무적으로 만들어주기
        int layer = LayerMask.NameToLayer("Player_NoneCol");
        gameObject.layer = layer;

        hp -= value;
        ChangeAnim(AnimationType.get_damage);
        if (hp <= 0)
        {
            // 플레이어 사망 처리
        }
        else
        {
            StartCoroutine(invincibility());
        }

        
    }

    IEnumerator invincibility()
    {
        invincibility_Time_Cur = invincibility_Time;

        while (invincibility_Time_Cur > 0)
        {
            AnimatorStateInfo stateInfo = anim.Animator.GetCurrentAnimatorStateInfo(0);

            // 현재 실행 중인 애니메이션인지 확인
            if (stateInfo.IsName("get_damage"))
            {
                if (stateInfo.normalizedTime >= 0.9f)
                {
                    // 히트 애니메이션 종료 후에는 대기 상태로 변경
                    AnimationState = AnimationType.stand;
                    anim.Animator.Play(AnimationState.ToString());
                }
            }
            invincibility_Time_Cur -= Time.deltaTime;
            yield return null;
        }

        // 무적 상태 종료
        int layer = LayerMask.NameToLayer("Player");
        gameObject.layer = layer;
    }





    public void LookMouse()
    {
        if(!IsOwner)
            return;
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

        rayDirection = new Vector3(transform.localScale.x, 0, 0);
    }

    // 클라이언트에서 호출되는 메서드
    void ChangeAnim(AnimationType newanim)
    {
        if (!IsOwner)
            return;

        if (IsClient)
        {
            // 서버에 애니메이션 변경 요청
            ChangeAnim_ServerRpc(newanim);
        }

        // 클라이언트에서 애니메이션 실행
        if (AnimationState == AnimationType.get_damage)
            return;
        anim.Animator.Play(newanim.ToString());

        // clip.name을 AnimationType으로 변환
        if (Enum.TryParse(typeof(AnimationType), newanim.ToString(), out var result))
        {
            AnimationState = (AnimationType)result;
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
        if (!IsOwner)
            return;

        if (AnimationState == AnimationType.get_damage)
            return;
        // 클라이언트에서 애니메이션 실행
        anim.Animator.Play(newanim.ToString());

        // clip.name을 AnimationType으로 변환
        if (Enum.TryParse(typeof(AnimationType), newanim.ToString(), out var result))
        {
            AnimationState = (AnimationType)result;
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




    void OnDrawGizmos()
    {
        
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + new Vector3(0,0.16f,0), transform.GetComponent<Collider2D>().bounds.size);
        
    }
}
