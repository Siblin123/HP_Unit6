using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfire : Item_Info
{
    [SerializeField] Light2D Light;

    [Header("x~y축 사이로 빛의 밝기가 바뀐다")]
    public Vector2 light_magnitude_Size;
    [SerializeField] float speed;
    string go_X_Y = "X";
    public override void Update()
    {
        base.Update();
        light_magnitude();
    }

    public void light_magnitude()
    {

        if(go_X_Y=="X")
        {
            Light.intensity += Time.deltaTime * speed;
        }
        else
        {
            Light.intensity -= Time.deltaTime * speed;
        }

        if (Light.intensity < light_magnitude_Size.x)
        {
            //커저야함
            go_X_Y="X";
        }
        else if(Light.intensity > light_magnitude_Size.y)
        {
            //작아저야함
            go_X_Y="Y";
        }
   
    }


}
