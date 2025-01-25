using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class All_Memory_Function : MonoBehaviour
{

    public List<Item_Info> all_Memory; 


    public void all_Function(Item_Info item)
    {
        switch (item.name)
        {
            case "총알의 기억":
                // 총알 관련 로직
                Debug.Log("총알의 기억이 발동되었습니다.");
                break;
            case "좋은 손전등의 기억":
                // 손전등 관련 로직
                Debug.Log("좋은 손전등의 기억이 발동되었습니다.");
                break;
            case "신발의 기억":
                // 신발 관련 로직
                Debug.Log("신발의 기억이 발동되었습니다.");
                break;
            case "점프팩의 기억":
                // 점프팩 관련 로직
                Debug.Log("점프팩의 기억이 발동되었습니다.");
                break;
            case "총의 기억":
                // 총 관련 로직
                Debug.Log("총의 기억이 발동되었습니다.");
                break;
            case "만능키의 기억":
                // 만능키 관련 로직
                Debug.Log("만능키의 기억이 발동되었습니다.");
                break;
            case "귀환석의 기억":
                // 귀환석 관련 로직
                Debug.Log("귀환석의 기억이 발동되었습니다.");
                break;
            case "도끼의 기억":
                // 도끼 관련 로직
                Debug.Log("도끼의 기억이 발동되었습니다.");
                break;
            case "옷의 기억":
                // 옷 관련 로직
                Debug.Log("옷의 기억이 발동되었습니다.");
                break;
            case "포션의 기억":
                // 포션 관련 로직
                Debug.Log("포션의 기억이 발동되었습니다.");
                break;
            case "노새의 기억":
                // 노새 관련 로직
                Debug.Log("노새의 기억이 발동되었습니다.");
                break;
            case "광부의 기억":
                // 광부 관련 로직
                Debug.Log("광부의 기억이 발동되었습니다.");
                break;
            case "나무꾼의 기억":
                // 나무꾼 관련 로직
                Debug.Log("나무꾼의 기억이 발동되었습니다.");
                break;
            case "무기 달인의 기억":
                // 무기 달인 관련 로직
                Debug.Log("무기 달인의 기억이 발동되었습니다.");
                break;
            case "저격수의 기억":
                // 저격수 관련 로직
                Debug.Log("저격수의 기억이 발동되었습니다.");
                break;
            case "연설가의 기억":
                // 연설가 관련 로직
                Debug.Log("연설가의 기억이 발동되었습니다.");
                break;
            case "심마니의 기억":
                // 심마니 관련 로직
                Debug.Log("심마니의 기억이 발동되었습니다.");
                break;
            case "강태공의 기억":
                // 강태공 관련 로직
                Debug.Log("강태공의 기억이 발동되었습니다.");
                break;
            case "스티브의 기억":
                // 스티브 관련 로직
                Debug.Log("스티브의 기억이 발동되었습니다.");
                break;
            case "헌터의 기억":
                // 헌터 관련 로직
                Debug.Log("헌터의 기억이 발동되었습니다.");
                break;
            default:
                // 기본 동작
                Debug.Log("알 수 없는 기억입니다.");
                break;
        }

    }


}
