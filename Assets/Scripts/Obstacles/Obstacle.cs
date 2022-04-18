    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Obstacle : MonoBehaviour
{
    protected bool _isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Box box))
        {
            box.RegisterHit(this);
            Destroy(gameObject);
        }
       
    }

    public abstract int RemoveBoxes(Box[]list, Box box, Action<Box> removeMethod);
}
