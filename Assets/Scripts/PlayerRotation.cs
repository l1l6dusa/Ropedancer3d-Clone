using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private Transform _source;
    [Range(0f,90f),SerializeField] 
    private float maxAvgAngle;
    [SerializeField]
    private Transform _constrainedBone;
    private float _widthScreen;
    private float _halfWidth;
    private Quaternion _initialRotation;
    private BoxLean _boxLean;

    public event Action<bool, float> CharacterRotated;

    public void Awake()
    {
        _boxLean = GetComponent<BoxLean>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _widthScreen = Screen.width;
        _halfWidth = _widthScreen / 2;
        _source.transform.rotation = _constrainedBone.rotation;
        _initialRotation = _source.transform.rotation;
        Debug.Log(_initialRotation.eulerAngles);
    }

    private void OnEnable()
    {
        //CharacterRotated += _boxLean.OnCharacterRotation;
        //CharacterRotated += _boxLean.OnCharacterRotation;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RotateCharacter();
        }
    }
    
    private void RotateCharacter()
    {
        var mousePosition = Input.mousePosition;
        float rotationOffsetRatio;
        if (mousePosition.x > _halfWidth)
        { 
            rotationOffsetRatio = (mousePosition.x - _halfWidth) / _halfWidth;
            //Debug.Log(rotationOffsetRatio);
            _source.transform.rotation =  Quaternion.Lerp(_initialRotation, Quaternion.Euler(new Vector3(_initialRotation.eulerAngles.x + maxAvgAngle, _initialRotation.eulerAngles.y, _initialRotation.eulerAngles.z)), rotationOffsetRatio);
            //CharacterRotated(true, rotationOffsetRatio);
        }
        else if(mousePosition.x < _halfWidth)
        { 
            rotationOffsetRatio = (_halfWidth - mousePosition.x) / _halfWidth;
            //Debug.Log(rotationOffsetRatio);
            _source.transform.rotation = Quaternion.Lerp(_initialRotation, Quaternion.Euler(new Vector3(_initialRotation.eulerAngles.x - maxAvgAngle, _initialRotation.eulerAngles.y, _initialRotation.eulerAngles.z)), rotationOffsetRatio);
            //CharacterRotated(false, rotationOffsetRatio);
        }
    }
}
