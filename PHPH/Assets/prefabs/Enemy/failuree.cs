using UnityEngine;

public class failuree : Enemy
{
    //1.특정 구역 반복

    void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();
    }
}
