using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NPOI.SS.Formula.PTG;

public class Make_Slot : NetworkBehaviour
{
    public Item_Info item;
    public Image item_I;

    public NetworkVariable<float> time = new NetworkVariable<float>();
    public TextMeshProUGUI time_T; // 제작 시간

    public void Update_Slot(Item_Info item)
    {
        if(item == null)
        {
            this.item = null;
            time.Value = 0;
            time_T.gameObject.SetActive(false);
            item_I.sprite = null;
        }
        else
        {
            this.item = item;
            item_I.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
            time.Value = 120f;
            time_T.gameObject.SetActive(true);
        }
    }

    public void Update()
    {
        if (time.Value > 0)
        {
            int minutes = Mathf.FloorToInt(time.Value / 60);
            int seconds = Mathf.FloorToInt(time.Value % 60);

            time_T.text = $"{minutes:D2}:{seconds:D2}"; // 2자리 숫자로 표시

            time.Value -= Time.deltaTime;
        }
        else
        {
            time_T.text = "완성!";
        }
    }

    public void Complet_B()
    {
        if(time.Value <= 0 && item != null)
        {
            if (csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Get_Item_OK(item, 1))
            {
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().Get_Item(item, 1);
                Update_Slot(null);
            }
        }
    }
}
