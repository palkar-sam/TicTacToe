using Aik.Libs.Observer;
using System.Collections;
using UnityEngine;

namespace Aik.Libs.StateMachine
{
    public interface IUIStateMachine
    {
        IUIState CurrentState { get; }
        Observable<IUIState> StateChangeObservable { get; set; }
        void InitializeStateMachine();
        T SwitchState<T>() where T : IUIState;
        void OnClickBack();
        T GetStateOfType<T>() where T : IUIState;
        IUIState GetPreviousState();
    }
}