using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(RigBuilder))]
public class Character : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private int _boxMaxDifference;
    [SerializeField] private ItemContainer _leftContainer;
    [SerializeField] private ItemContainer _rightContainer;
    [SerializeField] private Transform _ikSource;
    [Range(0f, 90f), SerializeField] private float _maxAvgAngle;
    [SerializeField] private Transform _constrainedBone;
    [SerializeField] private float _bendSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private float returnTime;
    [SerializeField] private float _horizontalOffset;
    [SerializeField] private List<Collider> _ragdollColliders;
    [SerializeField] private List<Collider> _gameLogicColliders;
    
    #endregion

    #region Private Fields

    private Animator _animator;
    private Rigidbody _rigidbody;

    #endregion
    
    #region Properties

    public Vector3 MinimumOffsetPosition { private set; get; }
    public Vector3 MaximumOffsetPosition { private set; get; }
    public float HorizontalOffsetDistance { private set; get; }

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public Quaternion InitialRotation { get; private set; }
    public Vector3 DefaultPosition { get; private set; }
    public Transform IkSource => _ikSource;
    public float MaxAvgAngle => _maxAvgAngle;
    public float BendSpeed => _bendSpeed;
    public float ReturnTime => returnTime;
    public float HorizontalOffset => _horizontalOffset;
    public int BoxQuantity => _leftContainer.BoxQuantity + _rightContainer.BoxQuantity;

    #endregion

    #region Events
    public event Action StarAdded;
    #endregion
    
    #region Unity Methods

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        SetupRagdoll();
        _ikSource.transform.rotation = _constrainedBone.rotation;
        InitialRotation = _constrainedBone.rotation;
        DefaultPosition = transform.position;
        MinimumOffsetPosition =
            new Vector3(DefaultPosition.x - _horizontalOffset, DefaultPosition.y, DefaultPosition.z);
        MaximumOffsetPosition =
            new Vector3(DefaultPosition.x + _horizontalOffset, DefaultPosition.y, DefaultPosition.z);
        HorizontalOffsetDistance = Mathf.Abs(MinimumOffsetPosition.x) + Mathf.Abs(MaximumOffsetPosition.x);
    }


    /*private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        ActivateRagdoll();
    }*/

    #endregion

    #region Ragdoll logic

    private void ActivateRagdoll()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _animator.enabled = false;
        _animator.avatar = null;
        _gameLogicColliders.ForEach(x => x.enabled = false);
        _ragdollColliders.ForEach(x=>
        {
            x.attachedRigidbody.velocity = Vector3.zero;
            x.attachedRigidbody.isKinematic = false;
            x.isTrigger = false;
            x.attachedRigidbody.useGravity = true;
        }
        );
    }

    private void SetupRagdoll()
    {
        _gameLogicColliders.Add(gameObject.GetComponent<Collider>());
        _gameLogicColliders.Add(_leftContainer.GetComponent<Collider>());
        _gameLogicColliders.Add(_rightContainer.GetComponent<Collider>());
        _ragdollColliders.AddRange(gameObject.GetComponentsInChildren<Collider>());
        _ragdollColliders.RemoveAll(i => _gameLogicColliders.Contains(i));
        _ragdollColliders.ForEach(x=>
        {
            x.attachedRigidbody.isKinematic = true;
            x.isTrigger = true;
            x.attachedRigidbody.useGravity = false;
        });
    }

    #endregion
    
    #region Boxes Logic

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

    public void RemoveBox()
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

    public void RemoveBoxes(int quantity, bool decrementActiveBoxes = true)
    {
        var leftSideQuantity = quantity / 2;
        var rightSideQuantity = quantity - leftSideQuantity;
        int boxDifference = _leftContainer.BoxQuantity - _rightContainer.BoxQuantity;
        if (_rightContainer.BoxQuantity > _leftContainer.BoxQuantity)
        {
            leftSideQuantity -= boxDifference;
            rightSideQuantity += boxDifference;
        }
        else
        {
            leftSideQuantity += boxDifference;
            rightSideQuantity -= boxDifference;
        }
        
        _leftContainer.RemoveBoxes(leftSideQuantity);
        _rightContainer.RemoveBoxes(rightSideQuantity);
    }

    public void AddBoxes(int quantity, bool incrementActiveBoxes = true)
    {
        var leftSideQuantity = quantity / 2;
        var rightSideQuantity = quantity - leftSideQuantity;
        var boxDifference = _leftContainer.BoxQuantity - _rightContainer.BoxQuantity;
        if (_rightContainer.BoxQuantity > _leftContainer.BoxQuantity)
        {
            leftSideQuantity += boxDifference;
            rightSideQuantity -= boxDifference;
        }
        else
        {
            leftSideQuantity -= boxDifference;
            rightSideQuantity += boxDifference;
        }
        _leftContainer.AddBoxes(leftSideQuantity);
        _rightContainer.AddBoxes(rightSideQuantity);
    }

    public void ProcessGate(GateModeEnum modeEnum, int value)
    {
        var modifiedBoxQuantity = BoxQuantity;
        switch (modeEnum)
        {
            case GateModeEnum.Addition:
                modifiedBoxQuantity += value;
                break;
            case GateModeEnum.Substraction:
                if (BoxQuantity == 0) return;
                modifiedBoxQuantity -= value;
                if (modifiedBoxQuantity < 0) modifiedBoxQuantity = 0;
                break;
            case GateModeEnum.Multiplication:
                modifiedBoxQuantity *= value;
                break;
            case GateModeEnum.Division:
                if (BoxQuantity<0) return;
                modifiedBoxQuantity /= value;
                break;
        }

        if (BoxQuantity > modifiedBoxQuantity)
        {
            RemoveBoxes(BoxQuantity-modifiedBoxQuantity);
        }
        else
        {
            AddBoxes(modifiedBoxQuantity-BoxQuantity);
        }
    }
    
    public float GetBoxRatio()
    {   
        if (_leftContainer.BoxQuantity == _rightContainer.BoxQuantity) return 0;
        var returnValue = Mathf.Clamp((float)(_leftContainer.BoxQuantity-_rightContainer.BoxQuantity)/_boxMaxDifference, -1, 1);
        return returnValue;
    }
    #endregion

    #region Subscription Methods
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
    #endregion

    #region Position Modifiers

    public void ModifyDefaultPosition(float x, float y, float z)
    {
        DefaultPosition += new Vector3(x, y, z);
    }

    #endregion

    #region Stars Logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Coin>(out _))
        {
            StarAdded?.Invoke();
        }
    }
    #endregion
    
    

    
}
