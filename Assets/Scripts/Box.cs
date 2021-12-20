using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Box : MonoBehaviour
{
    public event Action<Box, Obstacle> BoxCollided;
    

    public void RegisterHit(Obstacle obstacle)
    {
        BoxCollided(this, obstacle);
    }
    
    
}
