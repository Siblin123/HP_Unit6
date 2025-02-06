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


    [SerializeField] float fishing_time;//0이 되면 !가 뜸

    public override void Update()
    {
        base.Update();

        if(fishing_State!= Fishing_State.None)//낚시 중이 아니라면 움직일 수 있음
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
        //물에 가까이 있으면 낚시 가능
        /*
         e를 누르면 낚시 시작
          !<- 뜨면 n초 안에 e를 눌러야함
         
         */

        fishing_time = UnityEngine.Random.Range(0, 30);//물고기가 잡힐 시간
        fishing_State = Fishing_State.Fishing;

    }

    void Fishing_waiting()
    {
        if (fishing_State == Fishing_State.Fishing)
        {
            fishing_time -= Time.deltaTime;

            if(fishing_time<0)
            {
                //!가 뜸


                if (Input.GetKeyDown(KeyCode.E))
                {
                    //낚시 성공
                    if (Enum.TryParse(typeof(ArmType), "fishhook_attack_2_P", out var result))
                    {
                        csTable.Instance.gameManager.player.arm_Anim._anim = (ArmType)result;
                    }

                }
                
                if (fishing_time<-1f)
                {
                    //낚시 실패
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

