using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_Button : MonoBehaviour
{
    // ���� ��ư ����
    public static Shop_Button instance;

    private void Awake()
    {
        instance = this;
    }

    // ���� ��ư ����
    public void Exit_Button()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
