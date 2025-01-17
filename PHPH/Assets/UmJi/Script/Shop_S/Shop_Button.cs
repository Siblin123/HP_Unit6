using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_Button : MonoBehaviour
{
    // 상점 버튼 관련
    public static Shop_Button instance;

    private void Awake()
    {
        instance = this;
    }

    // 상점 버튼 관련
    public void Exit_Button()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
