using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float rotation = 5;
    
    private void FixedUpdate()
    {
        transform.Rotate(0,rotation,0);
    }
}
