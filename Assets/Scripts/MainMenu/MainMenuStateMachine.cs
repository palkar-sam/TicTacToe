using Aik.Libs.StateMachine;
using System.Collections;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuStateMachine : StateMachine
    {
        protected override void OnDestroyStateMachine()
        {
            Debug.Log("MainMenuStateMachine - OnDestroyStateMachine......");
        }

        protected override void OnExitStateMachine()
        {
            Debug.Log("MainMenuStateMachine - OnExitStateMachine......");
        }

        private IEnumerator Start()
        {
            InitializeStateMachine();
            yield return null;

            SwitchState<HomeViewPanel>();
        }
    }
}