using System;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] private int _boxMaxDifference; 
    [SerializeField] private ItemContainer _leftContainer;
    [SerializeField] private ItemContainer _rightContainer;
    [SerializeField] private Transform _ikSource;
    [Range(0f,90f),SerializeField] private float _maxAvgAngle;
    [SerializeField] private Transform _constrainedBone;
    [SerializeField] private float _bendSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float returnTime;
    [SerializeField] private float _horizontalOffset;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    
    
    public event Action StarAdded;
    public Quaternion InitialRotation { get; private set; }
    public Vector3 DefaultPosition { get; private set; }
    public Transform IkSource => _ikSource;
    public float MaxAvgAngle => _maxAvgAngle;
    public float BendSpeed => _bendSpeed;
    public float ReturnTime => returnTime;
    public float HorizontalOffset => _horizontalOffset;

    private void Awake()
    {
        _ikSource.transform.rotation = _constrainedBone.rotation;
        InitialRotation = _constrainedBone.rotation;
        DefaultPosition = transform.position;
    }


    public float GetBoxRatio()
    {   
        if (_leftContainer.BoxQuantity == _rightContainer.BoxQuantity) return 0;
        var returnValue = Mathf.Clamp((float)(_leftContainer.BoxQuantity-_rightContainer.BoxQuantity)/_boxMaxDifference, -1, 1);
        return returnValue;
    }

    public void MovementSubscribe(IState state)
    {
        _leftContainer.Subscribe(state);
        _rightContainer.Subscribe(state);
    }
 
    public void MovementUnsubscribe(IState state)
    {
        _leftContainer.Unsubscribe(state);
        _rightContainer.Unsubscribe(state);
    }

    public void HideBoxes()
    {
        _leftContainer.HideBoxes();
        _rightContainer.HideBoxes();
    }
    public void ShowBoxes()
    {
        _leftContainer.ShowBoxes();
        _rightContainer.ShowBoxes();
    }

    public void ModifyDefaultPosition(float x, float y, float z)
    {
        DefaultPosition += new Vector3(x, y, z);
    }

    public void DecrementBoxes()
    {
        if (_leftContainer.BoxQuantity > _rightContainer.BoxQuantity)
        {
            if (_leftContainer.BoxQuantity == 0)
            {
                return;
            }
            _leftContainer.RemoveLastBox();
            
        }
        else
        {
            if (_rightContainer.BoxQuantity == 0)
            {
                return;
            }
            _rightContainer.RemoveLastBox();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Coin>(out _))
        {
            StarAdded?.Invoke();
        }
    }
}
