using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Timing_Game : MonoBehaviour
{
    public GameObject treasurechest_block;
    public List<GameObject> treasurechest_blockList;

    public Transform point;

    public float pointSpeed = 100f;

    private float currentPointSpeed;

    private float pointAngle;


    public bool isGameStarted = false;

    GameObject curObj;

    void Start()
    {
        for(int i=0;  i< treasurechest_block.transform.childCount; i++)
        {
            treasurechest_blockList.Add(treasurechest_block.transform.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < treasurechest_block.transform.childCount; i++)
        {
            treasurechest_blockList.Add(treasurechest_block.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {

        if (!isGameStarted)
            return;

        pointAngle += currentPointSpeed * Time.deltaTime;

        if (pointAngle <= -262f)
        {
            pointAngle = -262f;
            currentPointSpeed = pointSpeed;  // Now rotate upward.
        }
        else if (pointAngle >= 0f)
        {
            pointAngle = 0f;
            currentPointSpeed = -pointSpeed; // Now rotate downward.
        }

        point.rotation = Quaternion.Euler(0f, 0f, pointAngle);

        if (Input.GetMouseButtonDown(0))
        {
            if (point.GetChild(0).GetComponent<Point_Block>().in_poinnt)
            {
                Rewrold();
               
            }
            else
            {
                Rewrold();
              
            }
        }
    }

    public void Rewrold()
    {

        switch (curObj.name)
        {
            case "RockDoor":
                curObj.GetComponent<Collider2D>().isTrigger = true;
                break;


        }
 
        pointAngle = 0f;
        isGameStarted = false;
        gameObject.SetActive(false);
    }

    public void startGame(GameObject obj)
    {
        curObj = obj;
        print("Game Started!");
        point.rotation = Quaternion.Euler(0f, 0f, 0f);

        foreach (GameObject b in treasurechest_blockList)
        {
            b.SetActive(false);
        }

        GameObject block = treasurechest_blockList[Random.Range(0, treasurechest_blockList.Count)];
        block.SetActive(true);

        float randomAngle = Random.Range(-90f, 45f);
        block.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
        isGameStarted = true;
    }

}
