using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallObstacle : Obstacle
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RemoveBoxes(List<Box> list, Box box, Action<Box> removeMethod)
    {
        var boxIndex = list.IndexOf(box);
        var boxesToRemove = list.Count - 1 - boxIndex;
        var tempList = list.FindAll(x => list.IndexOf(x) >= boxIndex);
        foreach (var itemBox in tempList)
        {
            itemBox.gameObject.SetActive(false);
            removeMethod(itemBox);
        }
    }
}
