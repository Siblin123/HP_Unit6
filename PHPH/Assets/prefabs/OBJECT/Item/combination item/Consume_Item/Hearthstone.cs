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

        //미니인벤토리에 있는 모든 아이템 버리기

        //인벤토리에서 아이템 제거(파괴)
    }
}
