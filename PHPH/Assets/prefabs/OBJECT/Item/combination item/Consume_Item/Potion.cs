using UnityEngine;

public class Potion : Item_Info
{
    public int hp_value;
    public int stamina_value;
    public int potoin_colTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void UseItem()
    {
        base.UseItem();
        Potion_F();
    }

    public void Potion_F()
    {
        csTable.Instance.gameManager.player.hp += hp_value;
        csTable.Instance.gameManager.player.stamina += stamina_value;

        if (csTable.Instance.gameManager.player.hp >= csTable.Instance.gameManager.player.maxHp)
        {
            csTable.Instance.gameManager.player.hp = csTable.Instance.gameManager.player.maxHp;
        }

        if(csTable.Instance.gameManager.player.stamina >= csTable.Instance.gameManager.player.maxStamina)
        {
            csTable.Instance.gameManager.player.stamina = csTable.Instance.gameManager.player.maxStamina;
        }

        colTime = potoin_colTime;
    }
}
