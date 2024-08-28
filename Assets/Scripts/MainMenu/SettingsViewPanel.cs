using Aik.Libs.StateMachine;
using UnityEngine;
using Views;

namespace MainMenu
{
    public class SettingsViewPanel : AnimatableState
    {
        protected override void OnDispose()
        {
            Debug.Log("SettingsViewPanel - OnDispose......");
        }

        protected override void OnHide()
        {
            Debug.Log("SettingsViewPanel - OnHide......");
        }

        protected override void OnInitialize()
        {
            Debug.Log("SettingsViewPanel - OnInitialize......");
        }

        protected override void OnShow()
        {
            Debug.Log("SettingsViewPanel - OnShow......");
        }

        protected override void OnUpdate()
        {
            Debug.Log("SettingsViewPanel - OnShow......");
        }
    }
}

