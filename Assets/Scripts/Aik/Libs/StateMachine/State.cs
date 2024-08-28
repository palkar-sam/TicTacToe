using Aik.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Aik.Libs.StateMachine
{
    public abstract class State : MonoBehaviour, IUIState, IBackListener
    {
        [SerializeField] private bool isInitialState;
        [SerializeField] private CustomButton backButton;

        bool IUIState.IsInitialState => isInitialState;
        bool IUIState.IsStackable => IsStackable;
        bool IUIState.OverrideBack => OverrideBack;

        

        /// <summary>
        /// Current statemachine.
        /// </summary>
        protected IUIStateMachine UiStateMachine;
        /// <summary>
        /// Use this flag if the state does not need to be stacked in the statemachine.
        /// </summary>
        protected bool IsStackable = true;
        /// <summary>
        /// Use this flag if the state doest need to listen to the hardware back button.
        /// </summary>
        protected bool OverrideBack;


        public void Init(IUIStateMachine statemachine)
        {
            UiStateMachine = statemachine;
            
            if (backButton != null)
            {
                backButton.AddListener(OnClickBack);
            }

            OnInitialize();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            OnHide();
            gameObject.SetActive(false);
        }

        public void Tick()
        {
            OnUpdate();
        }

        public void Dispose()
        {
            OnDispose();
            UiStateMachine = null;
        }

        #region Back Listeners 

        bool IBackListener.OverrideBack => throw new System.NotImplementedException();
        int IBackListener.Priority 
        {
            get
            {
                return 0;
            }
        }

        void IBackListener.OnBackEvent()
        {
            OnClickBack();
        }

        protected virtual void OnClickBack()
        {
            try
            {
                UiStateMachine.OnClickBack();
            }
            catch (NullReferenceException ex)
            {
                if (LoggerUtil.logEnabled) LoggerUtil.LogException(ex);
            }
        }

        #endregion

        /// <summary>
        /// Invoked after state is initialized and it is called only once.
        /// </summary>
        protected abstract void OnInitialize();
        /// <summary>
        /// Invoked after everytime state is enabled.
        /// </summary>
        protected abstract void OnShow();
        /// <summary>
        /// Invoked after evertime state is disabled.
        /// </summary>
        protected abstract void OnHide();
        /// <summary>
        /// Updates state every frame.
        /// </summary>
        protected abstract void OnUpdate();
        /// <summary>
        /// Invoked when state is destroyed.
        /// </summary>
        protected abstract void OnDispose();

        
    }
}