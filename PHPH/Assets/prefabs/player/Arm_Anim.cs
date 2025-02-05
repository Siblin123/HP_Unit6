
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System;
using Unity.Netcode.Components;
public class Arm_Anim : NetworkBehaviour
{
    public enum ArmType
    {
        empty_P,
        //도끼
        ax_stand_P,
        ax_attak_P,
        //곡괭이
        pickaxe_stand_P,
        pickaxe_attack_P,
        //낚시
        fishhook_stand_P,
        fishhook_attack_P,
        fishhook_attack_2_P,
        //횟불
        torchlight_stand_P,
        torxhlight_attack_P,
        //몽둥이
        bat_stand_P,
        bat_attack_P

    }


    public ArmType _anim;
    public NetworkAnimator arm_NetWorkAnimator;


    PlayerControl player;


    private void Start()
    {
        if(!IsOwner)
            return;
        player = csTable.Instance.gameManager.player.GetComponent<PlayerControl>();
    }
    public ArmType Anim
    {
        get => _anim;
        set
        {
            if (_anim != value) // 값이 변경될 때만 실행
            {
                _anim = value;
                ChangeAnim(_anim); // 값이 변경되면 자동으로 ChangeAnim 호출
            }
        }
    }


    public void Chage_Anim_Event(Arm_Anim.ArmType newStatue)//애니메이션 트리거로 상태변경
    {
        if(!IsOwner)
            return;

        csTable.Instance.gameManager.player.arm_Anim.Anim = newStatue;
    }


    public void UseItem_Event()
    {
        if (!IsOwner)
            return;
        player.UseCurItem_Attack();
    }


    // 클라이언트에서 호출되는 메서드
    public void ChangeAnim(ArmType newanim)
    {
        if (IsClient)
        {
            // 서버에 애니메이션 변경 요청
            ChangeAnim_ServerRpc(newanim);
        }

        // 클라이언트에서 애니메이션 실행
        arm_NetWorkAnimator.Animator.Play(newanim.ToString());

        // clip.name을 AnimationType으로 변환
        if (Enum.TryParse(typeof(ArmType), newanim.ToString(), out var result))
        {
            _anim = (ArmType)result;
        }
    }
    // 서버에서 호출되는 메서드
    [ServerRpc(RequireOwnership = false)]
    public void ChangeAnim_ServerRpc(ArmType newanim)
    {
        // 서버는 클라이언트에게 애니메이션 변경을 요청
        ChangeAnim_ClientRpc(newanim);
    }


    // 서버 -> 클라이언트로 애니메이션 변경 요청
    [ClientRpc]
    public void ChangeAnim_ClientRpc(ArmType newanim)
    {
        // 클라이언트에서 애니메이션 실행
        arm_NetWorkAnimator.Animator.Play(newanim.ToString());

        // clip.name을 AnimationType으로 변환
        if (Enum.TryParse(typeof(ArmType), newanim.ToString(), out var result))
        {
            _anim = (ArmType)result;
        }
    }

    private void Update()
    {
        if(!IsOwner)
            return;
       

    }
}
