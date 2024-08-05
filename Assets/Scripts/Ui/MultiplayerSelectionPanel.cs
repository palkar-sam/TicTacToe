using Palettes;
using Props;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MultiplayerSelectionPanel : PlayerSelectionPanel
    {
        [SerializeField] private TextMeshProUGUI inputText;
        [SerializeField] private Button createButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private PaletteView paletteView;

        public override void OnInitialize()
        {
            base.OnInitialize();

            createButton.onClick.AddListener(() => {
                SetVisibility(false);
                GameManager.Instance.AiColorCode = GameManager.Instance.UserColorCode = paletteView.SelectedColor;
                TriggerGameEvent(Props.GameEvents.ON_SHOW_MP_CREATEROOM); 
            });
            joinButton.onClick.AddListener(() => { 
                SetVisibility(false);
                GameManager.Instance.AiColorCode = GameManager.Instance.UserColorCode = paletteView.SelectedColor;
                TriggerGameEvent(Props.GameEvents.ON_SHOW_MP_JOINROOM); 
            });
        }

        public override void OnShow()
        {
            base.OnShow();
            inputText.text = string.Empty;
        }

        protected override void OnBackClick()
        {
            base.OnBackClick();
            NetworkManager.Instance.DisconnectToServer();
        }

        private void TriggerGameEvent(GameEvents eventName)
        {
            NetworkManager.Instance.ActiveUserName = string.IsNullOrEmpty(inputText.text) ? "ABC" : inputText.text;
            EventManager.TriggerEvent(eventName);
            NetworkManager.Instance.ConnectToServer();
        }
    }
}

