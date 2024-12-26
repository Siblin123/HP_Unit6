using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]

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
    public TextMeshProUGUI request_Count_T; // �ʿ��� ������ ����

    public Image reward_I; // ���� ������ �̹���
    public TextMeshProUGUI reward_Count_T; // ���� ����

    public Sprite money_I;

    private void Awake()
    {
        
        string[] rows = request_File.text.Split('\n'); // CSV�� �� ���� �и�
        for (int i = 1; i < rows.Length; i++) // ����� �ǳʶٰ� ������ �б�
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue; // �� �� ����
            string[] cols = rows[i].Split(','); // ��ǥ�� �и�
            Request request = new Request
            {
                reward = cols[0],
                item_Name = cols[1],
                item = GetComponent<Shop_Manager>().find_Item(cols[1]),
                item_Count = int.Parse(cols[2])
            };

            request_L.Add(request);
        }
    }

    private void Start()
    {
        Request_Selset();
    }

    public void Request_Selset() // �Ƿ� �̱�
    {
        int num = Random.Range(0, request_L.Count);
        select_R = request_L[num];

        // �ʿ��� ������ �̹���
        request_I.sprite = select_R.item.gameObject.GetComponent<SpriteRenderer>().sprite;
        // �ʿ��� ������ ����
        request_Count_T.text = select_R.item_Count.ToString();
        

        if (GetComponent<Shop_Manager>().find_Item(select_R.reward) == null) // ������ ���ΰ���
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
        }
    }
}
