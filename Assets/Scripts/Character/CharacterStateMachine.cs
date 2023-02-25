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
    private Animator Animator { get; set; }
    public InputTracker InputTracker { get; private set;}
    public IState CurrentState { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _character = GetComponent<Character>();
        InputTracker = GetComponent<InputTracker>();
    }

    private async void Start()
    {
        CurrentState = new TorsoRotationState();
        await CurrentState.InitializeStateAsync(_character, this, Animator, 0.5f);
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
                Animator, returnTime, 
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
                Animator, returnTime);
        }
        _character.MovementUnsubscribe(CurrentState);
        CurrentState = state;
        
        _character.MovementSubscribe(CurrentState);
    }

   
}
