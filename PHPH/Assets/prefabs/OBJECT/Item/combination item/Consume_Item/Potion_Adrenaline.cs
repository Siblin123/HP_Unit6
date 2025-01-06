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
        /*�÷��̾��� ü�°� ��� �Ҹ��� �����Ѵ�.���ҷ����� ���� ü���� ���� ��� ü���� 1�� ���´�.
            ü�� = -10
            ��� �Ҹ� ���� = 50 %
            ȿ������ = 10��
            ��Ÿ�� = 10��*/


        csTable.Instance.gameManager.player.stamina_percent = 50;
        StartCoroutine("effect_duration");
    }

    IEnumerator effect_duration()
    {
        yield return new WaitForSeconds(duration);
        csTable.Instance.gameManager.player.stamina_percent = 100;

    }
}
