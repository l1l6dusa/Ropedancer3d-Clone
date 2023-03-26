using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Character), typeof(InputTracker), typeof(Animator))]
public class CharacterStateMachine : MonoBehaviour
{
    private Task _task;
    private bool _userInputBlocked;
    private Character _character;
    private bool _isInputReceived;
    private Animator _animator { get; set; }
    public InputTracker InputTracker { get; private set;}
    public IState CurrentState { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _character = GetComponent<Character>();
        InputTracker = GetComponent<InputTracker>();
    }

    private async void Start()
    {
        CurrentState = new DefaultState();
        await CurrentState.InitializeStateAsync(_character, this, _animator, 0f);
    }

    private async void Update()
    {
        _isInputReceived = Input.GetMouseButton(0);
        if (_isInputReceived && !EventSystem.current.IsPointerOverGameObject() && !_userInputBlocked)
        {
            CurrentState?.UpdateAsync();
        }
        else
        {
            await CurrentState?.ZeroInputUpdateAsync();
        }
    }

    private async void FixedUpdate()
    {
        if (_isInputReceived && !EventSystem.current.IsPointerOverGameObject() && !_userInputBlocked)
        {
            CurrentState?.FixedUpdateAsync();
        }
        else
        {
            _task = CurrentState?.ZeroInputFixedUpdateAsync();
            await _task;

        }
    }
  

    public async void SetState(IState state, float returnTime, float externalReturnTime = float.NaN, Action postUpdateCallback=null, int ticks = 0)
    {
        if (CurrentState != null)
        {
            _userInputBlocked = true;
            
            await state.InitializeStateAsync(
                _character, 
                this, 
                _animator, returnTime, 
                CurrentState.ReturnToDefaultStateImmediateAsunc(), ticks);
            if (postUpdateCallback != null)
            {
                state.PostUpdateCallBack += postUpdateCallback;
            }

            _userInputBlocked = false;
        } 
        else
        {
            await state.InitializeStateAsync(
                _character, 
                this, 
                _animator, returnTime);
        }
        _character.MovementUnsubscribe(CurrentState);
        CurrentState = state;
        
        _character.MovementSubscribe(CurrentState);
    }

   
}
