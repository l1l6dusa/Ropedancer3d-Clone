using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public override int RemoveBoxes(Box[]  list, Box box, Action<Box> removeMethod)
    {
       if(_isTriggered) return 0;
       var temp = list.ToList().FindAll(x => x.gameObject.activeSelf);
        var boxIndex = list.ToList().IndexOf(box);
        var boxesToRemove = temp.Count - 1 - boxIndex;
        var tempList = temp.FindAll(x => temp.IndexOf(x) >= boxIndex);
        foreach (var itemBox in tempList)
        { 
            removeMethod(itemBox);
        }

        _isTriggered = true;
        return tempList.Count;
        

        



    }
}
