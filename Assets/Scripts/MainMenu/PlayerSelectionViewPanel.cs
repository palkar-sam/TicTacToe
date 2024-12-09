using Aik.Libs.StateMachine;
using Aik.Utils;
using Palettes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class PlayerSelectionViewPanel : AnimatableState
    {
        [SerializeField] private PaletteView colorPaletteView;
        [SerializeField] private CustomButton playButton;

        private string selectedColor;

        protected override void OnDispose()
        {
            LoggerUtil.Log("PlayerSelectionViewPanel - OnDispose......");
            playButton.RemoveListener();

        }

        protected override void OnHide()
        {
            LoggerUtil.Log("PlayerSelectionViewPanel - OnHide......");
        }

        protected override void OnInitialize()
        {
            LoggerUtil.Log("PlayerSelectionViewPanel - OnInitialize......");
            playButton.AddListener(OnPlayButtonClick);
        }

        protected override void OnShow()
        {
            LoggerUtil.Log("PlayerSelectionViewPanel - OnShow......");
        }

        protected override void OnUpdate()
        {
            LoggerUtil.Log("PlayerSelectionViewPanel - OnUpdate......");
        }

        private void OnPlayButtonClick()
        {
            GameManager.Instance.Is1vs1Enabled = false;
            GameManager.Instance.IsSinglePlayer = true;
            GameManager.Instance.UserColorCode = colorPaletteView.SelectedColor;
            GameManager.Instance.AiColorCode = PaletteView.GetAiColorCode(GameManager.Instance.UserColorCode);
            SceneManager.LoadSceneAsync(2);
        }
    }
}