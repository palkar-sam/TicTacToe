using Aik.Libs.Observer;
using System.Collections.Generic;
using UnityEngine;
using Aik.Utils;
using System;

namespace Aik.Libs.StateMachine
{
    public abstract class StateMachine : MonoBehaviour, IUIStateMachine
    {
        public string GetCurrentStateName
        {
            get
            {
                if (_currentState != null)
                {
                    return _currentState.GetType().Name;
                }
                else
                    return string.Empty;
            }
        }

        IUIState IUIStateMachine.CurrentState => throw new System.NotImplementedException();

        public Observable<IUIState> StateChangeObservable { get; set; }

        /// <summary>
        /// To avoid race conditions, this flag is used.
        /// </summary>
        bool _wasStateMachineInitialized = false;

        /// <summary>
        /// States under this statemachine.
        /// </summary>
        IUIState[] _states;

        /// <summary>
        /// Current shown state.
        /// </summary>
        IUIState _currentState;

        /// <summary>
        /// Stack of states used for maintaining visited history.
        /// </summary>
        Stack<IUIState> _stateStack;

        /// <summary>
        /// Queue which is used to handle quick transitions between different states without breaking the animation.
        /// </summary>
        Queue<IUIState> _statesToTransition;

        IUIState IUIStateMachine.GetPreviousState()
        {
            if (_stateStack.Count > 0)
            {
                IUIState previousState = _stateStack.Peek();
                return previousState;
            }
            return null;
        }

        T IUIStateMachine.GetStateOfType<T>()
        {
            if (_wasStateMachineInitialized)
            {
                int length = _states.Length;
                for (int i = 0; i < length; i++)
                {
                    if (_states[i] is T)
                    {
                        return (T)_states[i];
                    }
                }
            }
            return default(T);
        }

        void IUIStateMachine.InitializeStateMachine()
        {
            StateChangeObservable = new Observable<IUIState>(default(IUIState));

            _stateStack = new Stack<IUIState>(2);
            _statesToTransition = new Queue<IUIState>(1);

            int length = transform.childCount;
            List<IUIState> states = new List<IUIState>(length);

            for (int i = 0; i < length; i++)
            {
                var state = transform.GetChild(i).GetComponent<IUIState>();
                if (state != null)
                {
                    try
                    {
                        state.Init(this);
                        ((Component)state).gameObject.SetActive(false);
                        states.Add(state);
                        //if (state is IAnimatable)
                        //{
                        //    IAnimatable _animatable = (IAnimatable)state;

                        //    if (_animatable is AnimatableState)
                        //    {

                        //        AnimatableState animatableState = (AnimatableState)_animatable;

                        //        animatableState.SetTransitionLeftToRight();

                        //    }
                        //    //Debug.Log("transition setting done");
                        //}
                    }
                    catch (System.Exception ex)
                    {
                        if (LoggerUtil.logEnabled) LoggerUtil.LogException(ex);
                    }
                }
            }
            _states = states.ToArray();

            //_queueSystem = new AnimationQueueSystem();
            //_queueSystem.transistionHandler += AnimationQueueFinishHandler;

            _wasStateMachineInitialized = true;
        }

        void IUIStateMachine.OnClickBack()
        {
            if (_stateStack.Count > 0)
            {
                var state = _stateStack.Pop();

                //if (IsStateAnimating())
                //{
                //  _statesToTransition.Enqueue(state);
                //  return;
                //}

                if (state != null)
                {
                    SwitchState(state, false);
                }
                else
                {
                    throw new NullReferenceException("State doesn't exist in the stack.");
                }
            }
            else
            {
                OnExitStateMachine();
            }
        }

        T IUIStateMachine.SwitchState<T>()
        {
            var state = ((IUIStateMachine)this).GetStateOfType<T>();
            if (state != null)
            {
                /*if (IsStateAnimating() && !_currentState.Equals(state))
                {
                    _statesToTransition.Enqueue(state);
                }
                else*/
                {
                    SwitchState(state, true);
                }
                return state;
            }
            else
            {
                LoggerUtil.LogError(string.Format("State: {0} which is trying to access does not exist in the statemachine: {1}", typeof(T).Name, this.name));
                return default(T);
            }
        }

        /// <summary>
        /// Event when statemachine is exited.
        /// </summary>
        protected abstract void OnExitStateMachine();

        /// <summary>
        /// Event when statemachine is destroyed.
        /// </summary>
        protected abstract void OnDestroyStateMachine();

        protected void InitializeStateMachine()
        {
            ((IUIStateMachine)this).InitializeStateMachine();
        }

        protected T SwitchState<T>() where T : IUIState
        {
            return ((IUIStateMachine)this).SwitchState<T>();
        }

        protected void HideCurrentState()
        {
            if (_currentState != null)
            {
                //HideState(ref _currentState);
                _currentState = null;
            }
        }


        private void SwitchState(IUIState state, bool addToStack)
        {
            if (_currentState != null)
            {
                if (_currentState.Equals(state))
                {
                    //HideBlocker();
                    return;
                }

                HideState(ref _currentState);

                if (addToStack)
                    AddStateToStack(ref state);
            }

            //  Debug.Log(string.Format("CURRENT STATE :{0} NEXT STATE :{1}", _currentState, state));

            _currentState = state;
            StateChangeObservable.Value = state;

            if (_currentState.IsInitialState)
            {
                _stateStack.Clear();
            }

            ShowState(ref _currentState);
            //onShowScreen?.Invoke(GetCurrentStateName);
        }

        private void AddStateToStack(ref IUIState state)
        {
            //If stack already contains the state.
            //Remove all the states above it
            if (_stateStack.Contains(state))
            {
                RemoveStatesFromStackUpto(ref state);
            }

            if (_currentState.IsStackable)
            {
                _stateStack.Push(_currentState);
            }
        }

        private void RemoveStatesFromStackUpto(ref IUIState state)
        {
            //Pop all the states that are above the given state.
            while (_stateStack.Count > 0)
            {
                var poppedState = _stateStack.Pop();
                if (poppedState.Equals(state))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Hides the state.
        /// </summary>
        /// <param name="state">State</param>
        private void HideState(ref IUIState state)
        {
            if (state is IAnimatable)
            {
                //_queueSystem.Enqueue((IAnimatable)state, AnimationQueueSystem.AnimationType.Hide);
            }

            try
            {
                state.Hide();
            }
            catch (System.Exception e)
            {
                if (LoggerUtil.logEnabled) LoggerUtil.LogException(e);
            }
        }

        /// <summary>
        /// Reveals the state.
        /// </summary>
        /// <param name="state">State</param>
        private void ShowState(ref IUIState state)
        {
            if (state is IAnimatable)
            {
                //_queueSystem.Enqueue((IAnimatable)state, AnimationQueueSystem.AnimationType.Reveal);
            }

            try
            {
                state.Show();
            }
            catch (System.Exception e)
            {
                if (LoggerUtil.logEnabled) LoggerUtil.LogException(e);
                else Debug.LogError(e);
            }
        }
    }
}