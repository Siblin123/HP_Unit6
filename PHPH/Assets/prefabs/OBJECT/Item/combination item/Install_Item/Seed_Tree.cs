using UnityEngine;

public class Seed_Tree : Item_Info
{
    int curDay;
    private void Start()
    {
        curDay = csTable.Instance.gameManager.survivalDay.Value;
    }

    private void Update()
    {
        if(curDay== csTable.Instance.gameManager.survivalDay.Value)
        {

        }
    }

    void growTree()
    {

    }
}
