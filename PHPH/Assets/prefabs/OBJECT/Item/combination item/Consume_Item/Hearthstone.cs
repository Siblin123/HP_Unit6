using UnityEngine;

public class Hearthstone : Item_Info
{
    public override void UseItem()
    {
        base.UseItem();
        F_Hearthstone();
    }

    public void F_Hearthstone()
    {
        csTable.Instance.gameManager.player.curItem = null;
        //csTable.Instance.gameManager.player.transform.position = csTable.Instance.unitSopn_Pos.position;

        //�̴��κ��丮�� �ִ� ��� ������ ������

        //�κ��丮���� ������ ����(�ı�)
    }
}
