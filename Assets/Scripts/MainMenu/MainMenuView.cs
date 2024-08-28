using Model;
using Props;
using System;
using System.Collections.Generic;
using UnityEngine;
using Views;

namespace MainMenu
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private List<Screens> screens;

        private Screens _homeScreen;

        public override void OnShow()
        {
            base.OnShow();

            _homeScreen = screens[0];

            EventManager.StartListening(Props.GameEvents.ON_DISCONNECTED, OnDisconnected);
            EventManager<ScreenModel>.StartListening(Props.GameEvents.ON_SHOW_VIEW, OnShowDialog);
            EventManager<ScreenModel>.StartListening(Props.GameEvents.ON_CLOSE_VIEW, OnCloseDialog);
        }

        private void OnDestroy()
        {
            EventManager.StopListening(Props.GameEvents.ON_DISCONNECTED, OnDisconnected);
            EventManager<ScreenModel>.StopListening(Props.GameEvents.ON_SHOW_VIEW, OnShowDialog);
            EventManager<ScreenModel>.StopListening(Props.GameEvents.ON_CLOSE_VIEW, OnCloseDialog);
        }

        private void OnDisconnected()
        {
            _homeScreen.View.SetVisibility(true);
            (_homeScreen.View as HomeView).ShowLobby();
        }

        private void OnShowDialog(ScreenModel model)
        {
            _homeScreen.View.SetVisibility(false);
            Screens screen = screens.Find(itemView => itemView.Type == model.Type);
            screen.View.SetVisibility(true);
        }

        private void OnCloseDialog(ScreenModel model)
        {
            Screens screen = screens.Find(itemView => itemView.Type == model.Type);
            if(screen != null)
            {
                screen.View.SetVisibility(false);
                _homeScreen.View.SetVisibility(true);
            }
        }
    }

    [Serializable]
    public class Screens
    {
        [SerializeField] private ScreenType type;
        [SerializeField] private BaseView view;

        public ScreenType Type => type;
        public BaseView View => view;
    }
}