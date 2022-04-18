using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StationaryObstacle : Obstacle
{
    public override int RemoveBoxes(Box[] list, Box box, Action<Box> removeMethod)
    {
        if(_isTriggered) return 0;
        var tempList = list.ToList().FindAll(x => x.gameObject.activeSelf);
        if (tempList.Count <= 0) return 0;
        var removeThreshold = tempList.Count / 2;
        var removedBoxList = tempList.FindAll(x => tempList.IndexOf(x) >= removeThreshold );
        foreach (var itemBox in removedBoxList)
        { 
            removeMethod(itemBox);
        }
        _isTriggered = true;
        return tempList.Count;

    }
}
