using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;

public class All_Memory_Function : Item_Info
{

    public int max_Memory_Count; // 최대치

    public override void UseItem()
    {
        base.UseItem();

        int curCount = 0;
        foreach (string n in csTable.Instance.gameManager.player.have_Memory)
        {
            if (item_Name == n)
            {
                curCount++;
            }
        }

        if (curCount < max_Memory_Count)
        {

            csTable.Instance.gameManager.player.have_Memory.Add(item_Name);
            all_Function(item_Name);


            GetComponent<NetworkObject>().Despawn();

        }
        else
        {
            print("이미 최대치를 획득했습니다.");
        }



    }


    public bool returnBool(string itemName)
    {
        switch (itemName)
        {

            case "도끼의 기억":
                // 도끼 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;
                

            case "총알의 기억":
                // 총알 관련 로직
                Debug.Log("총알을 만들 수 있다. 최대치=1");
                return true;

            case "옷의 기억":
                // 옷 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;

            case "좋은 손전등의 기억":
                // 손전등 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;

            case "총의 기억":
                // 총 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;

            case "점프팩의 기억":
                // 점프팩 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;

            case "만능키의 기억":
                // 만능키 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;


            case "포션의 기억":
                // 포션 관련 로직
                Debug.Log("중첩으로 먹을 수 없다. 최대치=1");
                return true;

            case "귀환석의 기억":
                // 귀환석 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;


            case "신발의 기억":
                // 신발 관련 로직
                Debug.Log("만들 수 있다. 최대치=1");
                return true;


            default:
                return false;
        }
    }


    public void retrunFunction(string itemName)
    {
        switch (itemName)
        {
            case "나무꾼의 기억":
                // 나무꾼 관련 로직 ax_memory_Value 수정
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 쿨타임이 줄어든다.도끼 쿨타임-0.3초");
                csTable.Instance.gameManager.player.ax_memory_Value += 0.3f;
                break;

            case "광부의 기억":
                // 광부 관련 로직  pick_memory_Value 수정
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 쿨타임이 줄어든다. 곡괭이 쿨타임-0.3초");
                csTable.Instance.gameManager.player.pick_memory_Value += 0.3f;
                break;

            case "노새의 기억":
                // 노새 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 잠겨있던 아이템 칸 일부를 사용할 수 있게 된다.  최대치=6");
                break;

      
            case "연설가의 기억":
                // 연설가 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 상점의 판매 개수가 증가한다.    상점 판매 개수+1 최대치=6");
                csTable.Instance.shop_Manager.UnRock_Slot();
                break;

            case "저격수의 기억":
                // 저격수 관련 로직  longAttackDamegeUp_Value수정
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 능력치가 증가한다. 원거리 대미지+0.3");
                csTable.Instance.gameManager.player.longAttackDamegeUp_Value += 0.3f;
                break;

            case "무기 달인의 기억":
                // 무기 달인 관련 로직  shortAttackDamageUp_Value수정
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 능력치가 증가한다. 근접 데미지+0.3");
                csTable.Instance.gameManager.player.shortAttackDamageUp_Value += 0.3f;
                break;
        }

    }

    public void Gotget_Function(string itemName)
    {
        switch(itemName)
        {
            case "심마니의 기억":
                // 심마니 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 채집에서 낮은 확률로 산삼을 발견할 수 있다.");
                csTable.Instance.gameManager.player.get_gethering_Item_Per += 0.1f;
                break;

            case "강태공의 기억":
                // 강태공 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 낚시에서 낮은 확률로 상어를 낚을 수 있다.");
                csTable.Instance.gameManager.player.get_fishing_Item_Per += 0.1f;
                break;

            case "스티브의 기억":
                // 스티브 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 광질에서 낮은 확률로 다이아몬드를 캘 수 있다.");
                csTable.Instance.gameManager.player.get_mining_Item_Per += 0.1f;
                break;

            case "헌터의 기억":
                // 헌터 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 몬스터를 잡으면 보통의 확률로 값어치 1인 재료가 나오며, 매우 낮은 확률로 값어치가 100 미만인 아이템이 나온다.");
                csTable.Instance.gameManager.player.get_hunting_Item_Per += 0.1f;
                break;
        }
    }
    

    public void all_Function(string itemName)
    {

        switch (itemName)
        {
            case "도끼의 기억":
                // 도끼 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;

            case "총알의 기억":
                // 총알 관련 로직
                returnBool(itemName);
                Debug.Log("총알을 만들 수 있다. 최대치=1");
                break;

            case "옷의 기억":
                // 옷 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;

            case "좋은 손전등의 기억":
                // 손전등 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;

            case "총의 기억":
                // 총 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;

            case "점프팩의 기억":
                // 점프팩 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;

            case "만능키의 기억":
                // 만능키 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;


            case "포션의 기억":
                // 포션 관련 로직
                returnBool(itemName);
                Debug.Log("중첩으로 먹을 수 없다. 최대치=1");
                break;

            case "귀환석의 기억":
                // 귀환석 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;


            case "신발의 기억":
                // 신발 관련 로직
                returnBool(itemName);
                Debug.Log("만들 수 있다. 최대치=1");
                break;

            case "나무꾼의 기억":
                // 나무꾼 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 쿨타임이 줄어든다.도끼 쿨타임-0.3초");
                retrunFunction(itemName);
                break;

            case "광부의 기억":
                // 광부 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 쿨타임이 줄어든다. 곡괭이 쿨타임-0.3초");
                retrunFunction(itemName);
                break;

            case "노새의 기억":
                // 노새 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 잠겨있던 아이템 칸 일부를 사용할 수 있게 된다.  최대치=6");
                retrunFunction(itemName);
                break;


            case "연설가의 기억":
                // 연설가 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 상점의 판매 개수가 증가한다.    상점 판매 개수+1 최대치=6");
                retrunFunction(itemName);
                break;

            case "저격수의 기억":
                // 저격수 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 능력치가 증가한다. 원거리 대미지+0.3");
                retrunFunction(itemName);
                break;

            case "무기 달인의 기억":
                // 무기 달인 관련 로직
                Debug.Log("중첩으로 먹을 수 있으며, 먹을 때마다 능력치가 증가한다. 근접 데미지+0.3");
                retrunFunction(itemName);
                break;

            case "심마니의 기억":
                // 심마니 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 채집에서 낮은 확률로 산삼을 발견할 수 있다.");
                Gotget_Function(itemName);
                break;

            case "강태공의 기억":
                // 강태공 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 낚시에서 낮은 확률로 상어를 낚을 수 있다.");
                Gotget_Function(itemName);
                break;

            case "스티브의 기억":
                // 스티브 관련 로직
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 광질에서 낮은 확률로 다이아몬드를 캘 수 있다.");
                Gotget_Function(itemName);
                break;

            case "헌터의 기억":
                // 헌터 관련 로직
                Gotget_Function(itemName);
                Debug.Log("중첩으로 먹을 수 없으며, 먹을 시 몬스터를 잡으면 보통의 확률로 값어치 1인 재료가 나오며, 매우 낮은 확률로 값어치가 100 미만인 아이템이 나온다.");
                break;

            default:
                // 기본 동작
                Debug.Log("알 수 없는 기억입니다.");
                break;
        }
    }




}
