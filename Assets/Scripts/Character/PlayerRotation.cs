using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerRotation : MonoBehaviour
{
    
    [SerializeField] private Transform _source;
    [Range(0f,90f),SerializeField] private float _maxAvgAngle;
    [SerializeField] private Transform _constrainedBone;
    [SerializeField] private float _bendSpeed; 
    
    //todo move away _halfWidth to separate script
    private float _widthScreen;
    private float _halfWidth;
    private Quaternion _initialRotation;
    private Character _character;
    
    public event Action<float> MouseMoved;
    
    private void Start()
    {
        _character = GetComponent<Character>();
        _widthScreen = Screen.width;
        _halfWidth = _widthScreen / 2;
        _source.transform.rotation = _constrainedBone.rotation;
        _initialRotation = _source.transform.rotation;
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RotateCharacter();
        }
        else
        {
            AutomaticallyRotateCharacter();
        }
    }
    private void AutomaticallyRotateCharacter()
    {
        if (_character.GetBoxRatio() != 0)
        {
            _source.transform.rotation =
                Quaternion.Lerp(_source.transform.rotation, 
                    Quaternion.Euler(
                    new Vector3(
                        (_initialRotation.eulerAngles.x + _maxAvgAngle) * _character.GetBoxRatio(), 
                        _initialRotation.eulerAngles.y, 
                        _initialRotation.eulerAngles.z)), 
                    _bendSpeed * Time.deltaTime);
        }
        else
        {
            _source.transform.rotation =
                Quaternion.Lerp(_source.transform.rotation, _initialRotation, _bendSpeed * Time.deltaTime);
        }
        MouseMoved?.Invoke(-_character.GetBoxRatio());
    }

    private void RotateCharacter()
    {
        var mousePosition = Input.mousePosition;
        float rotationOffsetRatio;
        if (mousePosition.x > _halfWidth)
        {
            rotationOffsetRatio = (mousePosition.x - _halfWidth) / _halfWidth;
            _source.transform.rotation =  
                Quaternion.Lerp(_initialRotation, Quaternion.Euler(
                    new Vector3(_initialRotation.eulerAngles.x + _maxAvgAngle, _initialRotation.eulerAngles.y, _initialRotation.eulerAngles.z)),
                    rotationOffsetRatio);
            
            MouseMoved?.Invoke(Mathf.Clamp(-rotationOffsetRatio,-1f, 0f));
        }
        //todo create a separate method to calculate rotation offset 
        else if(mousePosition.x < _halfWidth)
        { 
            rotationOffsetRatio = (_halfWidth - mousePosition.x) / _halfWidth;
            _source.transform.rotation =
                Quaternion.Lerp(_initialRotation, Quaternion.Euler(
                    new Vector3(_initialRotation.eulerAngles.x - _maxAvgAngle, _initialRotation.eulerAngles.y, _initialRotation.eulerAngles.z)),
                    rotationOffsetRatio);
            
            MouseMoved?.Invoke(Mathf.Clamp(rotationOffsetRatio, 0f, 1f));
        }
        
        
    }
}
