using UnityEngine;

public class active_Item : combination_item
{
    public int damege;
    public float attack_rage;
    public float CrowdControl_Time;

    public LayerMask active_rayMask;
    RaycastHit2D [] hits;
    public baseStatus target;

    
    public override void UseItem()
    {
        base.UseItem();
        Attack();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, csTable.Instance.gameManager.player.rayDirection * attack_rage, Color.red, 0.5f);
    }

    public void Attack()
    {
        hits = Physics2D.RaycastAll(transform.position, csTable.Instance.gameManager.player.rayDirection, attack_rage, active_rayMask);

      
        if (hits.Length!=0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                //tag)Type�� Enemy�� ��츦 ���� ã�� �״��� Object�� ��츦 ã�´�.
                if (hits[i].transform.GetComponent<baseStatus>().tagType == baseStatus.tag_Type.Enemy)
                {
                    target = hits[i].transform.GetComponent<baseStatus>();
                    break;
                }
                else if (hits[i].transform.GetComponent<baseStatus>().tagType == baseStatus.tag_Type.Object)
                {
                    target = hits[i].transform.GetComponent<baseStatus>();
                }
            }

            if(target != null)
            {
                target.TakeDamage(damege);
            }
        }

      

    }
}
