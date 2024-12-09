using Aik.Libs.StateMachine;
using System.Collections;
using UnityEngine;
using Aik.Utils;
using System.Collections.Generic;

namespace MainMenu
{
    public class HomeViewPanel : AnimatableState
    {
        [SerializeField] private TopBarView uiTopBar;
        [SerializeField] private List<CustomButton> homeBtns;

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
            for (int i = 0; i < homeBtns.Count; i++)
            {
                homeBtns[i].AddListener(ShowModePanel, (i + 1));
            }

            ShowModePanel(-1);
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

        private void ShowModePanel(int mode)
        {
            switch(mode)
            {
                case 1:
                    UiStateMachine.SwitchState<PlayerSelectionViewPanel>();
                    break;
                case 2:
                    UiStateMachine.SwitchState<MultiPlayerSelectionViewPanel>();
                    break;
                case 3:
                    UiStateMachine.SwitchState<TwoPlayerSelectionViewPanel>();
                    break;
            }
        }
    }
}