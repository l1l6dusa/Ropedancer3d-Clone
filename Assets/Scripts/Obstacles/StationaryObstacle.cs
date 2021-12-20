using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StationaryObstacle : Obstacle
{
    public override void RemoveBoxes(List<Box> list, Box box, Action<Box> removeMethod)
    {
        if (list.Count > 0)
        {
            var removeThreshold = list.Count / 2;
            var tempList = list.FindAll(x => list.IndexOf(x) >= removeThreshold );
            foreach (var itemBox in tempList)
            { 
                itemBox.gameObject.SetActive(false);
                removeMethod(itemBox);
            }
        }
    }
}
