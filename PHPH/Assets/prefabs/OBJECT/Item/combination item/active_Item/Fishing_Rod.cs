using System;
using UnityEngine;
using static Arm_Anim;

public class Fishing_Rod : active_Item
{

    public enum Fishing_State
    {
        None,
        Fishing,
        Fishing_Game,
        Fishing_End
    }

  public Fishing_State fishing_State;

    public override void UseItem()
    {
        base.UseItem();
        Fishing();
        Debug.Log("Fishing_Rod");
    }

    [Header("���� ����")]
    public float fishing_time;//0�� �Ǹ� !�� ��
    [SerializeField] GameObject getFish_Image;

    public override void Update()
    {
        base.Update();

        testtttt();

        if (fishing_State!= Fishing_State.None)//���� ���� �ƴ϶�� ������ �� ����
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

    void testtttt()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
/*            if (Enum.TryParse(typeof(ArmType), "fishhook_attack_2_P", out var result))
            {
                csTable.Instance.gameManager.player.arm_Anim.Anim = (ArmType)result;

            }
            fishing_State = Fishing_State.None;
*/
            Fishing();
        }
           
    }
    public void Fishing()
    {
        //���� ������ ������ ���� ����
        /*
         e�� ������ ���� ����
          !<- �߸� n�� �ȿ� e�� ��������
         
         */

       fishing_time = UnityEngine.Random.Range(5, 30);//����Ⱑ ���� �ð�
        fishing_State = Fishing_State.Fishing;

    }

    void Fishing_waiting()
    {
        if (fishing_State == Fishing_State.Fishing)
        {
            fishing_time -= Time.deltaTime;

            if (fishing_time < 0)
            {
                //!�� ��
                getFish_Image.SetActive(true);
                getFish_Image.transform.position=csTable.Instance.gameManager.player.transform.position;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    getFish_Image.SetActive(false);
                    //���� ����
                    if (Enum.TryParse(typeof(ArmType), "fishhook_attack_2_P", out var result))
                    {
                        csTable.Instance.gameManager.player.arm_Anim.Anim = (ArmType)result;
                    }
                    fishing_State = Fishing_State.None;
                    print("����1");
                    return;
                    

                }
                
                if (fishing_time < -1f)
                {
                    if (Enum.TryParse(typeof(ArmType), "fishhook_attack_2_P", out var result))
                    {
                        csTable.Instance.gameManager.player.arm_Anim.Anim = (ArmType)result;
                    }

                    //���� ����
                    getFish_Image.SetActive(false);
                    fishing_State = Fishing_State.None; print("����2");
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Enum.TryParse(typeof(ArmType), "fishhook_attack_2_P", out var result))
                    {
                        csTable.Instance.gameManager.player.arm_Anim.Anim = (ArmType)result;
                    }
                    getFish_Image.SetActive(false);
                    //���� ����
                    getFish_Image.SetActive(false);
                    fishing_State = Fishing_State.None; print("����3");
                }
            }

        }
    }


    public void Fishing_Game()
    {
        Debug.Log("Fishing_Game");
    }

}

