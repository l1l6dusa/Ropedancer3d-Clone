    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Obstacle : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Box box))
        {
            box.RegisterHit(this);
            Destroy(gameObject);
        }
       
    }

    public abstract void RemoveBoxes(List<Box> list, Box box, Action<Box> removeMethod);
}
