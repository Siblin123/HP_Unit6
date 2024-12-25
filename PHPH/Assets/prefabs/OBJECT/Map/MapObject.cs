using UnityEngine;

public class MapObject : interaction
{
    public Item_Info reword_Item;
    int jumpingItem_Value = 2;


    public void getItem()
    {
        if (reword_Item != null)
        {
            GameObject item = Instantiate(reword_Item.gameObject, transform.position, Quaternion.identity);
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpingItem_Value), ForceMode2D.Impulse); // jumpingItem_Value를 사용하여 위로 힘을 가함
       
            Destroy(gameObject);
        }
    }

    public override void interact()
    {
        getItem();
    }
}
