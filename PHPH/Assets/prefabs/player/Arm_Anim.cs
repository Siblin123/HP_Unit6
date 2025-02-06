
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
        //����
        ax_stand_P,
        ax_attak_P,
        //���
        pickaxe_stand_P,
        pickaxe_attack_P,
        //����
        fishhook_stand_P,
        fishhook_attack_P,
        fishhook_attack_2_P,
        //Ƚ��
        torchlight_stand_P,
        torxhlight_attack_P,
        //������
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
            if (_anim != value) // ���� ����� ���� ����
            {
                _anim = value;
                ChangeAnim(_anim); // ���� ����Ǹ� �ڵ����� ChangeAnim ȣ��
            }
        }
    }


    public void Chage_Anim_Event(Arm_Anim.ArmType newStatue)//�ִϸ��̼� Ʈ���ŷ� ���º���
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


    // Ŭ���̾�Ʈ���� ȣ��Ǵ� �޼���
    public void ChangeAnim(ArmType newanim)
    {
        if (IsClient)
        {
            // ������ �ִϸ��̼� ���� ��û
            ChangeAnim_ServerRpc(newanim);
        }

        // Ŭ���̾�Ʈ���� �ִϸ��̼� ����
        arm_NetWorkAnimator.Animator.Play(newanim.ToString());

        // clip.name�� AnimationType���� ��ȯ
        if (Enum.TryParse(typeof(ArmType), newanim.ToString(), out var result))
        {
            _anim = (ArmType)result;
        }
    }
    // �������� ȣ��Ǵ� �޼���
    [ServerRpc(RequireOwnership = false)]
    public void ChangeAnim_ServerRpc(ArmType newanim)
    {
        // ������ Ŭ���̾�Ʈ���� �ִϸ��̼� ������ ��û
        ChangeAnim_ClientRpc(newanim);
    }


    // ���� -> Ŭ���̾�Ʈ�� �ִϸ��̼� ���� ��û
    [ClientRpc]
    public void ChangeAnim_ClientRpc(ArmType newanim)
    {
        // Ŭ���̾�Ʈ���� �ִϸ��̼� ����
        arm_NetWorkAnimator.Animator.Play(newanim.ToString());

        // clip.name�� AnimationType���� ��ȯ
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
