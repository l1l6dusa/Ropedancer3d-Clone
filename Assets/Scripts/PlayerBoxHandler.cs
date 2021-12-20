using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//TODO generalize the script, in order to use in for separate pickUpPoints
public class PlayerBoxHandler : MonoBehaviour
{
    [SerializeField] private Transform _attachLeft;
    [SerializeField] private Transform _attachRight;
    [SerializeField] private GameObject _pickUpPrefab;
    [SerializeField] private TriggerProxy _triggerLeft;
    [SerializeField] private TriggerProxy _triggerRight;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform _source;
    [SerializeField] private CustomSliderWrapping sliderWrapping;
    [Range(0,1),SerializeField] private float _boxWeight;
    
    private List<GameObject> _itemsLeft;
    private List<GameObject> _itemsRight;

    private event Action<float> _boxAdded;
   
    
    private void Start()
    {
        _itemsLeft = new List<GameObject>();
        _itemsRight = new List<GameObject>();
    }
    
    private void OnEnable()
    {
        _triggerLeft.TriggerEntered += OnTriggerEnterProxyLeft;
        _triggerRight.TriggerEntered += OnTriggerEnterProxyRight;
        _boxAdded += sliderWrapping.UpdateSlider;
    }

    private void OnDisable()
    {
        _triggerLeft.TriggerEntered -= OnTriggerEnterProxyLeft;
        _triggerRight.TriggerEntered -= OnTriggerEnterProxyRight;
        _boxAdded -= sliderWrapping.UpdateSlider;
    }

    private void OnTriggerEnterProxyLeft(Collider other)
    {
        Debug.Log("Left Trigger");
        InitializeBox(_attachLeft, _itemsLeft);
        _boxAdded(-_boxWeight);
    }
    
    private void OnTriggerEnterProxyRight(Collider other)
    {
        Debug.Log("Right Trigger");
        InitializeBox(_attachRight, _itemsRight);
        _boxAdded(_boxWeight);
    }

    private void InitializeBox(Transform attachPoint, List<GameObject> _boxList)
    {
        if (_boxList.Count == 0)
        {
            var temp = Instantiate(_pickUpPrefab, attachPoint);
            temp.transform.localPosition = offset/2;
            _boxList.Add(temp);
        }
        else
        {
            var temp = Instantiate(_pickUpPrefab, attachPoint);
            temp.transform.localPosition = offset * _boxList.Count;
            _boxList.Add(temp);
        }
    }
}
