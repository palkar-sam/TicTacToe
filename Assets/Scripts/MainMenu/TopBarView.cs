using Model;
using System;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace MainMenu
{
    public class TopBarView : BaseView
    {
        [SerializeField] private Button settingButton;

        public event Action OnShowSettings;
        public override void OnInitialize()
        {
            base.OnInitialize();

            settingButton.onClick.AddListener(OnShowSettingButton);
        }

        private void OnShowSettingButton()
        {
            OnShowSettings?.Invoke();
        }
    }
}

