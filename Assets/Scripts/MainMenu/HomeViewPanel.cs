using Aik.Libs.StateMachine;
using System.Collections;
using UnityEngine;
using Aik.Utils;

namespace MainMenu
{
    public class HomeViewPanel : AnimatableState
    {
        [SerializeField] private TopBarView uiTopBar;
        protected override void OnDispose()
        {
            LoggerUtil.Log("HomePanelView - OnDispose......");
            uiTopBar.OnShowSettings -= OnShowSettings;
        }

        protected override void OnHide()
        {
            Debug.Log("HomePanelView - OnHide......");
        }

        protected override void OnInitialize()
        {
            Debug.Log("HomePanelView - OnInitialize......");

            uiTopBar.OnShowSettings += OnShowSettings;
        }

        protected override void OnShow()
        {
            Debug.Log("HomePanelView - OnShow......");
        }

        protected override void OnUpdate()
        {
            Debug.Log("HomePanelView - OnShow......");
        }

        private void OnShowSettings()
        {
            UiStateMachine.SwitchState<SettingsViewPanel>();
        }
    }
}