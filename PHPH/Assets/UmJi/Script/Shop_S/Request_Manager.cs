using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;
using System;

[System.Serializable]
/*���� �̸�, �䱸 ������ �̸�, �䱸 ������ ����
200, ���,1
100, �� ����,3
100, Ȳ��,2*/
public class Request
{
    public string reward; // ���� -> ���ϼ��� �ְ� �������ϼ��� ����
    public string item_Name; // �ʿ��� ������ �̸�
    public Item_Info item; // �ʿ��� ������ ����
    public int item_Count; // �ʿ��� ������ ����
}

public class Request_Manager : MonoBehaviour
{
    public List<Request> request_L;
    public TextAsset request_File;
    public Request select_R; // ���� �Ƿ�
    public Item_Info reward_Item; // ���� ������

    public Image request_I; // �ʿ��� ������ �̹���
    //public TextMeshProUGUI request_Count_T; // �ʿ��� ������ ����

    //public Image reward_I; // ���� ������ �̹���
    public int reward_Count;
    public TextMeshProUGUI reward_Count_T; // ���� ����


    public int request_DeadLine;
    public TextMeshProUGUI request_DeadLine_T; // �̼� ����


    public Sprite money_I;

    private void Awake()
    {
        request_DeadLine = 2;

        //string[] rows = request_File.text.Split('\n'); // CSV�� �� ���� �и�
        string[] rows = request_File.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < rows.Length; i++) // ����� �ǳʶٰ� ������ �б�
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue; // �� �� ����
            string[] cols = rows[i].Split(','); // ��ǥ�� �и�
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
            request_DeadLine_T.text = "����: " + request_DeadLine.ToString() + "��";
        }
    }

    public void Request_Selset() // �Ƿ� �̱�
    {
        int num = UnityEngine.Random.Range(0, request_L.Count );
        select_R = request_L[num];

        // �ʿ��� ������ �̹���
        request_I.sprite = select_R.item.gameObject.GetComponent<SpriteRenderer>().sprite;
        // �ʿ��� ������ ����
        //request_Count_T.text = select_R.item_Count.ToString();

        reward_Count = int.Parse(select_R.reward);
        reward_Count_T.text = select_R.reward;

        /*if (GetComponent<Shop_Manager>().find_Item(select_R.reward) == null) // ������ ���ΰ���
        {
            reward_Item = null;

            // ���� �̹��� -> �� �̹���
            reward_I.sprite = money_I;

            // ���� ���� -> �� �ݾ�
            reward_Count_T.text = select_R.reward;
        }
        else // ������ ������ �ΰŰ�
        {
            reward_Item = GetComponent<Shop_Manager>().find_Item(select_R.reward);

             // ���� �̹��� -> ������ �̹���
            reward_I.sprite = reward_Item.gameObject.GetComponent<SpriteRenderer>().sprite;

            // ���� ���� -> �ִ� ���� ����
            reward_Count_T.text = reward_Item.max_Have_Count.ToString();
        }*/
    }
}
