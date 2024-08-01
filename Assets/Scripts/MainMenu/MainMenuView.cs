using UnityEngine;
using Views;

namespace MainMenu
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private CreateView createView;
        [SerializeField] private JoinView joinView;
        [SerializeField] private HomeView homeView;

        public override void OnShow()
        {
            base.OnShow();

            createView.SetVisibility(false);
            joinView.SetVisibility(false);
            //homeView.SetVisibility(true);

            //createView.OnHide += OnShowHomeView;
            //joinView.OnHide += OnShowHomeView;

            EventManager.StartListening(Props.GameEvents.ON_SHOW_MP_CREATEROOM, OnShowCreateDialog);
            EventManager.StartListening(Props.GameEvents.ON_SHOW_MP_JOINROOM, OnShowJoinDialog);
            EventManager.StartListening(Props.GameEvents.ON_DISCONNECTED, OnDisconnected);
        }

        private void OnDestroy()
        {
            EventManager.StopListening(Props.GameEvents.ON_SHOW_MP_CREATEROOM, OnShowCreateDialog);
            EventManager.StopListening(Props.GameEvents.ON_SHOW_MP_JOINROOM, OnShowJoinDialog);
            EventManager.StopListening(Props.GameEvents.ON_DISCONNECTED, OnDisconnected);
        }

        private void OnDisconnected()
        {
            OnShowHomeView();
            homeView.ShowLobby();
        }

        private void OnShowCreateDialog()
        {
            createView.SetVisibility(true);
            homeView.SetVisibility(false);
            joinView.SetVisibility(false);
        }

        private void OnShowJoinDialog()
        {
            joinView.SetVisibility(true);
            createView.SetVisibility(false);
            homeView.SetVisibility(false);
        }

        private void OnShowHomeView()
        {
            joinView.SetVisibility(false);
            createView.SetVisibility(false);
            homeView.SetVisibility(true);
        }
        
    }
}

