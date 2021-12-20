using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[ExecuteInEditMode] 
public class TestDistance : MonoBehaviour
{
    private Transform[] _objects;
    // Start is called before the first frame update
    void Start()
    {

        _objects = transform.GetComponentsInChildren<Transform>().Where(x => x.TryGetComponent<Box>(out Box box)).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount>1)
        {
            GetLocalDistance();
        }
    }
    
    private void GetLocalDistance()
    {
        Debug.Log((_objects[1].localPosition - _objects[0].localPosition)/2);
    }
}
