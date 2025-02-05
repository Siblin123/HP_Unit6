using UnityEngine;

public class RockDoor : baseStatus
{
    public Timing_Game timing_Game;

    public override void interact()
    {
        if (GetComponent<Collider2D>().isTrigger==false /*그리고 열쇠가 있다면 */)
        {
            base.interact();
            //열쇠를 하나 잃어버린다
            timing_Game.gameObject.SetActive(true);
            timing_Game.startGame(gameObject);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
