using UnityEngine;

public class RockDoor : baseStatus
{
    public Timing_Game timing_Game;

    public override void interact()
    {
        if (GetComponent<Collider2D>().isTrigger==true)
        {
            base.interact();
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
