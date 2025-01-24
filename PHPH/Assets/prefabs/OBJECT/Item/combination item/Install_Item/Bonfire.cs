using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bonfire : Item_Info
{
    [SerializeField] Light2D Light;

    [Header("x~y�� ���̷� ���� ��Ⱑ �ٲ��")]
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
            //Ŀ������
            go_X_Y="X";
        }
        else if(Light.intensity > light_magnitude_Size.y)
        {
            //�۾�������
            go_X_Y="Y";
        }
   
    }


}
