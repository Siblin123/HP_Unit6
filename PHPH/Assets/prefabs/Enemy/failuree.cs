using UnityEngine;

public class failuree : Enemy
{
    //1.Ư�� ���� �ݺ�

    void Update()
    {
        Random_movement();
        WallCheck2D();
        Find_Player();
    }
}
