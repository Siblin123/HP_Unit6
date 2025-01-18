using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System;

[System.Serializable]
/*보상 이름, 요구 아이템 이름, 요구 아이템 개수
200, 상어,1
100, 쓴 열매,3
100, 황동,2*/
public class Request
{
    public string reward; // 보상 -> 돈일수도 있고 아이템일수도 있음
    public string item_Name; // 필요한 아이템 이름
    public Item_Info item; // 필요한 아이템 정보
    public int item_Count; // 필요한 아이템 개수
}

public class Request_Manager : MonoBehaviour
{
    public List<Request> request_L;
    public TextAsset request_File;
    public Request select_R; // 뽑힌 의뢰
    public Item_Info reward_Item; // 보상 아이템

    public Image request_I; // 필요한 아이템 이미지
    //public TextMeshProUGUI request_Count_T; // 필요한 아이템 개수

    //public Image reward_I; // 보상 아이템 이미지
    public int reward_Count;
    public TextMeshProUGUI reward_Count_T; // 보상 개수


    public int request_DeadLine;
    public TextMeshProUGUI request_DeadLine_T; // 미션 기한


    public Sprite money_I;

    private void Awake()
    {
        request_DeadLine = 2;

        //string[] rows = request_File.text.Split('\n'); // CSV의 각 줄을 분리
        string[] rows = request_File.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < rows.Length; i++) // 헤더를 건너뛰고 데이터 읽기
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue; // 빈 줄 무시
            string[] cols = rows[i].Split(','); // 쉼표로 분리
            Request request = new Request
            {
                reward = cols[0],
                item_Name = cols[1],
                item = GetComponent<Shop_Manager>().find_Item(cols[1]),
            };
            //request.item_Count = request.item.max_Have_Count;
            request_L.Add(request);
        }
    }

    private void Start()
    {
        Request_Selset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            request_DeadLine--;
            if (request_DeadLine == 0)
            {
                request_DeadLine = 2;
                Request_Selset();
            }
            request_DeadLine_T.text = "기한: " + request_DeadLine.ToString() + "일";
        }
    }

    public void Request_Selset() // 의뢰 뽑기
    {
        int num = UnityEngine.Random.Range(0, request_L.Count );
        select_R = request_L[num];

        // 필요한 아이템 이미지
        request_I.sprite = select_R.item.gameObject.GetComponent<SpriteRenderer>().sprite;
        // 필요한 아이템 개수
        //request_Count_T.text = select_R.item_Count.ToString();

        reward_Count = int.Parse(select_R.reward);
        reward_Count_T.text = select_R.reward;

        /*if (GetComponent<Shop_Manager>().find_Item(select_R.reward) == null) // 없으면 돈인거임
        {
            reward_Item = null;

            // 보상 이미지 -> 돈 이미지
            reward_I.sprite = money_I;

            // 보상 개수 -> 돈 금액
            reward_Count_T.text = select_R.reward;
        }
        else // 있으면 아이템 인거고
        {
            reward_Item = GetComponent<Shop_Manager>().find_Item(select_R.reward);

             // 보상 이미지 -> 아이템 이미지
            reward_I.sprite = reward_Item.gameObject.GetComponent<SpriteRenderer>().sprite;

            // 보상 개수 -> 최대 소지 개수
            reward_Count_T.text = reward_Item.max_Have_Count.ToString();
        }*/
    }
}
