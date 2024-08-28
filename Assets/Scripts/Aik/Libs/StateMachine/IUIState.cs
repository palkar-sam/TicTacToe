using System.Collections;
using UnityEngine;

namespace Aik.Libs.StateMachine
{
    public interface IUIState : System.IDisposable
    {
        bool IsInitialState { get; }

        bool IsStackable { get; }
        /// <summary>
        /// check whether to listen to back.
        /// </summary>
        bool OverrideBack { get; }
        /// <summary>
        /// Initialize.
        /// </summary>
        void Init(IUIStateMachine statemachine);
        /// <summary>
        /// Enables the state.
        /// </summary>
        void Show();
        /// <summary>
        /// Disables the state.
        /// </summary>
        void Hide();
        /// <summary>
        /// Updates every frame.
        /// </summary>
        void Tick();
    }
}