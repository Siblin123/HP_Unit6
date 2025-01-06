using System.Collections;
using UnityEngine;

public class Potion_Adrenaline : Potion
{
    public float duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public override void UseItem()
    {
        base.UseItem();
        Adrenaline();
    }

    public void Adrenaline()
    {
        /*플레이어의 체력과 기력 소모량이 감소한다.감소량보다 현재 체력이 낮을 경우 체력은 1만 남는다.
            체력 = -10
            기력 소모량 감소 = 50 %
            효과지속 = 10초
            쿨타임 = 10초*/


        csTable.Instance.gameManager.player.stamina_percent = 50;
        StartCoroutine("effect_duration");
    }

    IEnumerator effect_duration()
    {
        yield return new WaitForSeconds(duration);
        csTable.Instance.gameManager.player.stamina_percent = 100;

    }
}
