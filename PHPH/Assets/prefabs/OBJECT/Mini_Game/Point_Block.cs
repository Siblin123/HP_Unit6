using UnityEngine;

public class Point_Block : MonoBehaviour
{
    public bool in_poinnt = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        in_poinnt = true;
       

    } 

    private void OnTriggerExit2D(Collider2D collision)
    {
        in_poinnt = false;

    }
}
