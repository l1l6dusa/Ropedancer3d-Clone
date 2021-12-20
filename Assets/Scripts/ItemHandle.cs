using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemHandle  : MonoBehaviour
{
    [SerializeField] private Transform _attachPoint;
    [SerializeField] private Box _pickUpPrefab;

    [SerializeField] private Vector3 offset;
    [Range(-1,1),SerializeField] private float _boxWeight;
    
    private List<Box> _items;


    public IEnumerable Items { get;}

    public event Action<float> _boxAdded;

    private void Start()
    {
        _items = new List<Box>();
    }

    private void OnTriggerEnterProxy(Collider other)
    {
        Debug.Log("Trigger");
        InitializeBox(_attachPoint, _items);
        _boxAdded(-_boxWeight);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        InitializeBox(_attachPoint, _items);
        _boxAdded(-_boxWeight);
    }

    private void InitializeBox(Transform attachPoint, List<Box> _boxList)
    {
        if (_boxList.Count == 0)
        {
            var temp = Instantiate(_pickUpPrefab, attachPoint);
            temp.transform.localPosition = offset/2;
            temp.BoxCollided += OnBoxCollided;
            _boxList.Add(temp);
            
        }
        else
        {
            var temp = Instantiate(_pickUpPrefab, attachPoint);
            temp.transform.localPosition = offset * _boxList.Count + offset/2;
            temp.BoxCollided += OnBoxCollided;
            _boxList.Add(temp);
        }
    }

    private void OnBoxCollided(Box box, Obstacle obstacle)
    {
        obstacle.RemoveBoxes(_items, box, RemoveBox);
        
    }

    private void RemoveBox(Box box)
    {
        box.BoxCollided -= OnBoxCollided;
        _items.Remove(box);
        Destroy(box);
    }
    
    
    
}
