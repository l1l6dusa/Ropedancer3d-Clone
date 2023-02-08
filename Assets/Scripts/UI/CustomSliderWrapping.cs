using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CustomSliderWrapping : MonoBehaviour
{
    
    [SerializeField]
    private ItemContainer itemHandleRight;
    [SerializeField]
    private ItemContainer itemHandleLeft;
    [SerializeField]
    private float _boxWeight;
    
    private void OnEnable()
    {
        itemHandleRight.BoxQuantityChanged += UpdateSlider;
        itemHandleLeft.BoxQuantityChanged += UpdateSlider;
    }
    

    private void OnDisable()
    {
        itemHandleRight.BoxQuantityChanged -= UpdateSlider;
        itemHandleLeft.BoxQuantityChanged -= UpdateSlider; 
    }

    private Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        
        _slider.minValue = -1;
        _slider.maxValue = 1;
        _slider.value = 0;
    }

    private void UpdateSlider(int incrementValue)
    {
        _slider.value += incrementValue/_boxWeight;
    }
    
}
