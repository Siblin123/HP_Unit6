using System;
using UnityEngine;
using static Arm_Anim;

public class Fishing_Rod : active_Item
{

    enum Fishing_State
    {
        None,
        Fishing,
        Fishing_Game,
        Fishing_End
    }

    Fishing_State fishing_State;

    public override void UseItem()
    {
        base.UseItem();
        Fishing();
        Debug.Log("Fishing_Rod");
    }


    [SerializeField] float fishing_time;//0�� �Ǹ� !�� ��

    public override void Update()
    {
        base.Update();

        if(fishing_State!= Fishing_State.None)//���� ���� �ƴ϶�� ������ �� ����
        {
            csTable.Instance.gameManager.player.GetComponent<PlayerControl>().enabled = false;
        }
        else
        {
            if (csTable.Instance.gameManager.player.GetComponent<PlayerControl>().enabled==false)
                csTable.Instance.gameManager.player.GetComponent<PlayerControl>().enabled = true;
        }

        if (fishing_State == Fishing_State.Fishing)
        {
            Fishing_waiting();
        }

    }

    public void Fishing()
    {
        //���� ������ ������ ���� ����
        /*
         e�� ������ ���� ����
          !<- �߸� n�� �ȿ� e�� ��������
         
         */

        fishing_time = UnityEngine.Random.Range(0, 30);//����Ⱑ ���� �ð�
        fishing_State = Fishing_State.Fishing;

    }

    void Fishing_waiting()
    {
        if (fishing_State == Fishing_State.Fishing)
        {
            fishing_time -= Time.deltaTime;

            if(fishing_time<0)
            {
                //!�� ��


                if (Input.GetKeyDown(KeyCode.E))
                {
                    //���� ����
                    if (Enum.TryParse(typeof(ArmType), "fishhook_attack_2_P", out var result))
                    {
                        csTable.Instance.gameManager.player.arm_Anim._anim = (ArmType)result;
                    }

                }
                
                if (fishing_time<-1f)
                {
                    //���� ����
                    fishing_State = Fishing_State.None;
                }
            }
        }
    }


    public void Fishing_Game()
    {
        Debug.Log("Fishing_Game");
    }

}

