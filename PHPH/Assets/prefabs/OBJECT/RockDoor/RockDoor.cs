using Unity.Netcode;
using UnityEngine;

public class RockDoor : baseStatus
{
    public Timing_Game timing_Game;

    public override void interact()
    {
        if (csTable.Instance.gameManager.player.curItem == null)
            return;

        if (GetComponent<Collider2D>().isTrigger==false && (csTable.Instance.gameManager.player.curItem.id== 113 || csTable.Instance.gameManager.player.curItem.id == 119))
        {
            base.interact();

            if (csTable.Instance.gameManager.player.curItem.GetComponent<NetworkObject>().IsSpawned)
                csTable.Instance.gameManager.player.curItem.GetComponent<NetworkObject>().Despawn(true);
            else
                csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().currentSlot].item = null;

            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().slot_List[csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().currentSlot].Update_Slot(null,0);
            csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().miri_List[csTable.Instance.gameManager.player.GetComponent<Player_Inventory>().currentSlot].Update_Slot(null, 0);
            //¿­¼è¸¦ ÇÏ³ª ÀÒ¾î¹ö¸°´Ù
            timing_Game.gameObject.SetActive(true);
            timing_Game.startGame(gameObject);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
