using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class stair : MonoBehaviour
{ 
    float VerticalInput;
    bool readyClimb;


    public Collider2D Collider2D;
    public List<Vector3> enterPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider2D = GetComponent<Collider2D>();
    }



    void Update()
    {
  
    }


}
