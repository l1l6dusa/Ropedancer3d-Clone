using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxLean : MonoBehaviour
{
    [SerializeField] private Transform _leftHandContainer;
    [SerializeField] private Transform _rightHandContainer;
    [SerializeField, Range(0f, 0.4f)] private float _xOffset;
    private List<Transform> _leftHandObjects;
    private List<Transform> _rightHandObjects;
    private List<Vector3> _leftPositions = new List<Vector3>();
    private List<Vector3> _rightPositions = new List<Vector3>();

    private void Start()
    {
        //hardcode
        _leftHandObjects = _leftHandContainer.GetComponentsInChildren<Transform>().Where(x => x.TryGetComponent<Box>(out var boxTemp) && x != transform.GetChild(0)).ToList();
        for (int i = 0; i < _leftHandObjects.Count; i++)
        {
            _leftPositions.Add(_leftHandObjects[i].localPosition);
        }
        _rightHandObjects = _rightHandContainer.GetComponentsInChildren<Transform>().Where(x => x.TryGetComponent<Box>(out var boxTemp) && x!= transform.GetChild(0)).ToList();
        for (int i = 0; i < _rightHandObjects.Count; i++)
        {
            _rightPositions.Add(_leftHandObjects[i].localPosition);
        }
    }


    public void OnCharacterRotation(bool isNegative, float offsetRatio)
    {
        if (isNegative)
        {
            for (var index = 0; index < _leftHandObjects.Count; index++)
            {
                var obj = _leftHandObjects[index];
                obj.localPosition = Vector3.Lerp(_leftPositions[index], new Vector3(-_xOffset, 0, 0)+_leftPositions[index], offsetRatio);
            }

            for (var index = 0; index < _rightHandObjects.Count; index++)
            {
                var obj = _rightHandObjects[index];
                obj.localPosition = Vector3.Lerp(_rightPositions[index], new Vector3(-_xOffset, 0, 0)+_rightPositions[index], offsetRatio);
            }
        }
        else
        {
            for (var index = 0; index < _leftHandObjects.Count; index++)
            {
                var obj = _leftHandObjects[index];
                obj.localPosition = Vector3.Lerp(_leftPositions[index], new Vector3(_xOffset, 0, 0)+_leftPositions[index], offsetRatio);
            }

            for (var index = 0; index < _rightHandObjects.Count; index++)
            {
                var obj = _rightHandObjects[index];
                obj.localPosition = Vector3.Lerp(_rightPositions[index], new Vector3(_xOffset, 0, 0)+_rightPositions[index], offsetRatio);
            }
        }
    }
}
