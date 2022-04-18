using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int _boxMaxDifference; 
    [SerializeField] private ItemContainer _leftContainer;
    [SerializeField] private ItemContainer _rightContainer;
    
    

    public float GetBoxRatio()
    {
        if (_leftContainer.BoxQuantity == _rightContainer.BoxQuantity) return 0;
        if(_leftContainer.BoxQuantity>_rightContainer.BoxQuantity)
        {
            return -Mathf.Clamp((float)(_leftContainer.BoxQuantity-_rightContainer.BoxQuantity)/_boxMaxDifference, 0, 1);
        }
        return Mathf.Clamp((float)(_rightContainer.BoxQuantity-_leftContainer.BoxQuantity)/_boxMaxDifference, 0, 1);
         
    }
}
