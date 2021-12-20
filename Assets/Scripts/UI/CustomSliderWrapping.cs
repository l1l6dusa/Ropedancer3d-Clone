using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CustomSliderWrapping : MonoBehaviour
{

    [SerializeField]
    private ItemHandle itemHandleRight;
    [SerializeField]
    private ItemHandle itemHandleLeft;

    private void OnEnable()
    {
        itemHandleRight._boxAdded += UpdateSlider;
        itemHandleLeft._boxAdded += UpdateSlider;
    }

    private void OnDisable()
    {
        itemHandleRight._boxAdded -= UpdateSlider;
        itemHandleLeft._boxAdded -= UpdateSlider; 
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

    public void UpdateSlider(float incrementValue)
    {
        _slider.value += incrementValue;
    }
    
}
