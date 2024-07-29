using Board;
using Palettes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Views;

namespace MainMenu
{
    public class HomeView : BaseView
    {
        [SerializeField] private GameObject buttonsContainer;
        [SerializeField] private List<CustomButton> homeBtns;
        [SerializeField] private List<PlayerSelectionPanel> modeSeletionPanels;

        public event Action OnCreate;
        public event Action OnOpenJoinDialog;

        public override void OnInitialize()
        {
            base.OnInitialize();

            for (int i = 0; i < homeBtns.Count; i++)
            {
                homeBtns[i].AddListener(ShowModePanel, i);
                modeSeletionPanels[i].OnHide += () => { buttonsContainer.SetActive(true); };
            }

            ShowModePanel(-1);
        }

        public void ShowCreateDialog()
        {
            OnCreate?.Invoke();
        }

        public void ShowJoinDialog()
        {
            OnOpenJoinDialog?.Invoke();
        }

        public void PlaySinglePlayerRound(int mode)
        {
            GameManager.Instance.Is1vs1Enabled = mode == 1;
            GameManager.Instance.IsSinglePlayer = mode == 0;

            if (string.IsNullOrEmpty(CardBoard.SelectedCode))
            {
                GameManager.Instance.UserColorCode = CardBoard.SelectedCode = PaletteView.DefaultColor;
            }

            GameManager.Instance.AiColorCode = PaletteView.GetAiColorCode(GameManager.Instance.UserColorCode);
            SceneManager.LoadSceneAsync(2);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < homeBtns.Count; i++)
            {
                homeBtns[i].RemoveListener();
                modeSeletionPanels[i].OnHide -= () => { ShowModePanel(-1); };
            }
        }

        private void ShowModePanel(int mode)
        {
            for (int i = 0; i < modeSeletionPanels.Count; i++)
            {
                modeSeletionPanels[i].SetVisibility(mode == i);
            }

            buttonsContainer.SetActive(mode == -1);

            switch (mode)
            {
                case 1:
                    GameManager.Instance.IsMultiplayer = true;
                    NetworkManager.Instance.ConnectToServer();
                    break;
                default:
                    LoggerUtil.Log("Home Panel : Incorrect button index : " + mode);
                    break;
            }

        }
    }
}

